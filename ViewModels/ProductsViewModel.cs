using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class ProductsViewModel : ObservableObject
{
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private bool _isAddPanelOpen;
    [ObservableProperty] private string _editTitle = string.Empty;
    [ObservableProperty] private string _editBarcode = string.Empty;
    [ObservableProperty] private string _editCategory = string.Empty;
    [ObservableProperty] private decimal _editPurchasePrice;
    [ObservableProperty] private decimal _editSalePrice;
    [ObservableProperty] private decimal _editStock;
    [ObservableProperty] private decimal _editMinStock;
    [ObservableProperty] private UnitType _editUnit = UnitType.Number;

    public ObservableCollection<Product> Products { get; } = new();
    public UnitType[] UnitTypes { get; } = Enum.GetValues<UnitType>();

    public ProductsViewModel()
    {
        LoadProducts();
    }

    public void LoadProducts()
    {
        using var db = DatabaseInitializer.CreateDbContext();
        Products.Clear();
        foreach (var p in db.Products.AsNoTracking().ToList())
            Products.Add(p);
    }

    [RelayCommand]
    private void ToggleAddPanel() => IsAddPanelOpen = !IsAddPanelOpen;

    [RelayCommand]
    private void SaveProduct()
    {
        if (string.IsNullOrWhiteSpace(EditTitle)) return;
        using var db = DatabaseInitializer.CreateDbContext();
        var product = new Product
        {
            Title = EditTitle,
            Barcode = string.IsNullOrWhiteSpace(EditBarcode) ? null : EditBarcode,
            Category = EditCategory,
            Unit = EditUnit,
            PurchasePrice = EditPurchasePrice,
            SalePrice = EditSalePrice,
            Stock = EditStock,
            MinStock = EditMinStock,
            WarehouseId = 1
        };
        db.Products.Add(product);
        db.SaveChanges();
        IsAddPanelOpen = false;
        ClearForm();
        LoadProducts();
    }

    [RelayCommand]
    private void DeleteProduct(Product? product)
    {
        if (product == null) return;
        using var db = DatabaseInitializer.CreateDbContext();
        var p = db.Products.Find(product.Id);
        if (p != null)
        {
            p.IsActive = false;
            db.SaveChanges();
        }
        LoadProducts();
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsAddPanelOpen = false;
        ClearForm();
    }

    private void ClearForm()
    {
        EditTitle = string.Empty;
        EditBarcode = string.Empty;
        EditCategory = string.Empty;
        EditPurchasePrice = 0;
        EditSalePrice = 0;
        EditStock = 0;
        EditMinStock = 0;
        EditUnit = UnitType.Number;
    }
}
