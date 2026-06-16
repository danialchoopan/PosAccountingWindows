using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.Views;

public partial class PosView : UserControl
{
    private ObservableCollection<PosCartItem> _cart = new();
    private bool _isUpdating;
    private List<Product> _allProducts = new();

    public PosView()
    {
        InitializeComponent();
        LoadProducts();
        LoadPaymentMethods();
        Loaded += (_, _) =>
        {
            FullyPaidChk.IsChecked = true;
            UpdateTotals();
        };
    }

    private void LoadProducts()
    {
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            _allProducts = db.Products.AsNoTracking().Where(p => p.IsActive).ToList();
        }
        catch { }
    }

    private void LoadPaymentMethods()
    {
        PaymentCombo.Items.Clear();
        PaymentCombo.Items.Add(new ComboBoxItem { Content = "نقدی" });
        PaymentCombo.Items.Add(new ComboBoxItem { Content = "کارتخوان" });
        PaymentCombo.Items.Add(new ComboBoxItem { Content = "نسیه" });
        PaymentCombo.Items.Add(new ComboBoxItem { Content = "اقساطی" });
        PaymentCombo.Items.Add(new ComboBoxItem { Content = "ترکیبی" });
        PaymentCombo.SelectedIndex = 0;
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_isUpdating) return;
        var query = SearchBox.Text?.Trim() ?? "";
        SearchResultsList.Items.Clear();

        if (string.IsNullOrEmpty(query) || query.Length < 1)
        {
            SearchDropdown.Visibility = Visibility.Collapsed;
            return;
        }

        var matches = _allProducts.Where(p =>
            p.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            (p.Barcode != null && p.Barcode.Contains(query)) ||
            (p.Category != null && p.Category.Contains(query, StringComparison.OrdinalIgnoreCase))
        ).Take(10).ToList();

        foreach (var p in matches)
            SearchResultsList.Items.Add(p);

        SearchDropdown.Visibility = matches.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    private void SearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Down && SearchResultsList.Items.Count > 0)
        {
            SearchResultsList.SelectedIndex = 0;
            SearchResultsList.Focus();
            e.Handled = true;
        }
        else if (e.Key == Key.Enter)
        {
            if (SearchResultsList.SelectedItem is Product p)
                AddToCart(p);
            else if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                SearchByBarcode(SearchBox.Text.Trim());
        }
        else if (e.Key == Key.Escape)
        {
            SearchDropdown.Visibility = Visibility.Collapsed;
        }
    }

    private void SearchResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SearchResultsList.SelectedItem is Product p)
            AddToCart(p);
    }

    private void SearchResultsList_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && SearchResultsList.SelectedItem is Product p)
            AddToCart(p);
    }

    private void SearchByBarcode(string barcode)
    {
        var product = _allProducts.FirstOrDefault(p =>
            p.Barcode != null && p.Barcode == barcode);
        if (product != null) AddToCart(product);
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            SearchByBarcode(SearchBox.Text.Trim());
    }

    private void AddToCart(Product product)
    {
        var existing = _cart.FirstOrDefault(c => c.ProductId == product.Id);
        if (existing != null)
        {
            existing.Quantity++;
            existing.Subtotal = existing.Quantity * existing.UnitPrice;
        }
        else
        {
            _cart.Add(new PosCartItem
            {
                ProductId = product.Id,
                ProductTitle = product.Title,
                Quantity = 1,
                UnitPrice = product.SalePrice,
                PurchasePrice = product.PurchasePrice,
                Subtotal = product.SalePrice
            });
        }

        CartGrid.ItemsSource = null;
        CartGrid.ItemsSource = _cart;
        _isUpdating = true;
        SearchBox.Text = "";
        _isUpdating = false;
        SearchDropdown.Visibility = Visibility.Collapsed;
        SearchBox.Focus();
        UpdateTotals();
    }

    private void IncBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is PosCartItem item)
        {
            item.Quantity++;
            item.Subtotal = item.Quantity * item.UnitPrice;
            UpdateTotals();
        }
    }

    private void DecBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is PosCartItem item)
        {
            if (item.Quantity > 1)
            {
                item.Quantity--;
                item.Subtotal = item.Quantity * item.UnitPrice;
            }
            else
                _cart.Remove(item);

            UpdateTotals();
        }
    }

    private void DelBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is PosCartItem item)
        {
            var result = MessageBox.Show(
                $"آیا از حذف '{item.ProductTitle}' از سبد اطمینان دارید؟",
                "تایید حذف", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _cart.Remove(item);
                UpdateTotals();
            }
        }
    }

    private void FullyPaidChk_Checked(object sender, RoutedEventArgs e)
    {
        if (PaidAmountBox == null) return;
        var total = CalculateTotal();
        PaidAmountBox.Text = total.ToString("N0");
        UpdateChange();
    }

    private void FullyPaidChk_Unchecked(object sender, RoutedEventArgs e)
    {
        if (PaidAmountBox == null) return;
        PaidAmountBox.Text = "0";
        UpdateChange();
    }

    private void FinalizeBtn_Click(object sender, RoutedEventArgs e)
    {
        if (_cart.Count == 0)
        {
            MessageBox.Show("سبد خرید خالی است", "توجه", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var total = CalculateTotal();
        var result = MessageBox.Show(
            $"آیا از ثبت فاکتور به مبلغ {total:N0} ریال اطمینان دارید؟",
            "تایید فروش", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            var sale = new Sale
            {
                InvoiceNumber = $"INV-{DateTime.Now:yyyyMMdd}-{DateTime.Now:HHmmss}",
                UserId = AppSettings.CurrentUser?.Id ?? 1,
                Subtotal = _cart.Sum(c => c.Subtotal),
                TaxAmount = _cart.Sum(c => c.Subtotal) * 0.10m,
                TotalAmount = total,
                TotalNetProfit = _cart.Sum(c => (c.UnitPrice - c.PurchasePrice) * c.Quantity),
                PaymentMethod = PaymentMethod.Cash,
                CustomerPaid = decimal.TryParse(PaidAmountBox.Text?.Replace(",", ""), out var paid) ? paid : 0,
                ChangeAmount = (decimal.TryParse(PaidAmountBox.Text?.Replace(",", ""), out var p2) ? p2 : 0) - total,
                CreatedAt = DateTime.Now,
                Status = SaleStatus.Normal
            };

            foreach (var item in _cart)
            {
                sale.Items.Add(new SaleItem
                {
                    ProductId = item.ProductId,
                    ProductTitle = item.ProductTitle,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    PurchasePrice = item.PurchasePrice,
                    Subtotal = item.Subtotal
                });

                var product = db.Products.Find(item.ProductId);
                if (product != null) product.Stock -= item.Quantity;
            }

            db.Sales.Add(sale);
            db.SaveChanges();

            var invoiceWin = new InvoicePreviewWindow(sale);
            invoiceWin.ShowDialog();

            _cart.Clear();
            CartGrid.ItemsSource = null;
            UpdateTotals();
        }
        catch (Exception ex)
        {
            App.LogError(ex);
            MessageBox.Show("خطا: " + ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private decimal CalculateTotal()
    {
        var subtotal = _cart.Sum(c => c.Subtotal);
        var tax = subtotal * 0.10m;
        return subtotal + tax;
    }

    private void UpdateTotals()
    {
        var subtotal = _cart.Sum(c => c.Subtotal);
        var tax = subtotal * 0.10m;
        var total = subtotal + tax;

        SubtotalText.Text = $"{subtotal:N0}";
        TaxText.Text = $"{tax:N0}";
        TotalText.Text = $"{total:N0}";

        if (FullyPaidChk.IsChecked == true)
            PaidAmountBox.Text = total.ToString("N0");

        UpdateChange();
    }

    private void UpdateChange()
    {
        var total = CalculateTotal();
        if (decimal.TryParse(PaidAmountBox.Text?.Replace(",", ""), out var paid))
            ChangeText.Text = $"{paid - total:N0}";
        else
            ChangeText.Text = $"-{total:N0}";
    }
}

public class PosCartItem
{
    public int ProductId { get; set; }
    public string ProductTitle { get; set; } = "";
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal Subtotal { get; set; }
}
