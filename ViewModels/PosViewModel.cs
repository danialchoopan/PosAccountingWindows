using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class PosViewModel : ObservableObject
{
    [ObservableProperty] private string _barcodeInput = string.Empty;
    [ObservableProperty] private decimal _subtotal;
    [ObservableProperty] private decimal _taxAmount;
    [ObservableProperty] private decimal _totalAmount;
    [ObservableProperty] private decimal _customerPaid;
    [ObservableProperty] private decimal _changeAmount;
    [ObservableProperty] private string _customerName = string.Empty;
    [ObservableProperty] private PaymentMethod _selectedPaymentMethod = PaymentMethod.Cash;

    public ObservableCollection<PosCartItem> CartItems { get; } = new();
    public PaymentMethod[] PaymentMethods { get; } = Enum.GetValues<PaymentMethod>();

    [RelayCommand]
    private void AddByBarcode()
    {
        if (string.IsNullOrWhiteSpace(BarcodeInput)) return;

        using var db = DatabaseInitializer.CreateDbContext();
        var product = db.Products.FirstOrDefault(p =>
            p.Barcode == BarcodeInput || p.Title == BarcodeInput);

        if (product == null) return;

        var existing = CartItems.FirstOrDefault(i => i.ProductId == product.Id);
        if (existing != null)
        {
            existing.Quantity++;
            existing.Subtotal = existing.Quantity * existing.UnitPrice;
        }
        else
        {
            CartItems.Add(new PosCartItem
            {
                ProductId = product.Id,
                ProductTitle = product.Title,
                Quantity = 1,
                UnitPrice = product.SalePrice,
                PurchasePrice = product.PurchasePrice,
                Subtotal = product.SalePrice
            });
        }

        BarcodeInput = string.Empty;
        Recalculate();
    }

    [RelayCommand]
    private void RemoveItem(PosCartItem? item)
    {
        if (item == null) return;
        CartItems.Remove(item);
        Recalculate();
    }

    [RelayCommand]
    private void FinalizeSale()
    {
        if (CartItems.Count == 0) return;

        using var db = DatabaseInitializer.CreateDbContext();

        var sale = new Sale
        {
            InvoiceNumber = $"INV-{DateTime.Now:yyyyMMdd}-{DateTime.Now:HHmmss}",
            UserId = 1,
            Subtotal = Subtotal,
            TaxAmount = TaxAmount,
            TotalAmount = TotalAmount,
            CustomerPaid = CustomerPaid,
            ChangeAmount = ChangeAmount,
            PaymentMethod = SelectedPaymentMethod,
            CreatedAt = DateTime.Now,
            Items = CartItems.Select(i => new SaleItem
            {
                ProductId = i.ProductId,
                ProductTitle = i.ProductTitle,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                PurchasePrice = i.PurchasePrice,
                Subtotal = i.Subtotal
            }).ToList()
        };

        // Update stock
        foreach (var item in CartItems)
        {
            var product = db.Products.Find(item.ProductId);
            if (product != null)
                product.Stock -= item.Quantity;
        }

        db.Sales.Add(sale);
        db.SaveChanges();

        // Clear
        CartItems.Clear();
        Subtotal = 0;
        TaxAmount = 0;
        TotalAmount = 0;
        CustomerPaid = 0;
        ChangeAmount = 0;
        CustomerName = string.Empty;
    }

    [RelayCommand]
    private void SuspendInvoice()
    {
        // TODO: Save suspended invoice to DB
        CartItems.Clear();
        Subtotal = 0;
        TotalAmount = 0;
    }

    private void Recalculate()
    {
        Subtotal = CartItems.Sum(i => i.Subtotal);
        TaxAmount = Subtotal * 0.10m;
        TotalAmount = Subtotal + TaxAmount;
        ChangeAmount = CustomerPaid - TotalAmount;
    }
}

public partial class PosCartItem : ObservableObject
{
    public int ProductId { get; set; }
    [ObservableProperty] private string _productTitle = string.Empty;
    [ObservableProperty] private decimal _quantity;
    [ObservableProperty] private decimal _unitPrice;
    [ObservableProperty] private decimal _purchasePrice;
    [ObservableProperty] private decimal _subtotal;
}
