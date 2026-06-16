using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class ProductsViewModel : ObservableObject
{
    private List<Product> _allProducts = new();

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
    [ObservableProperty] private string _errorMessage = string.Empty;

    public ObservableCollection<Product> Products { get; } = new();
    public ObservableCollection<string> Categories { get; } = new();
    public UnitType[] UnitTypes { get; } = Enum.GetValues<UnitType>();

    public ProductsViewModel() { LoadProducts(); LoadCategories(); }

    public void LoadProducts()
    {
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            _allProducts = db.Products.AsNoTracking().Where(p => p.IsActive).OrderBy(p => p.Title).ToList();
            ApplyFilter();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    public void LoadCategories()
    {
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            Categories.Clear();
            foreach (var c in db.Categories.Where(c => c.IsActive).OrderBy(c => c.SortOrder).Select(c => c.Name).ToList())
                Categories.Add(c);
        }
        catch { }
    }

    partial void OnSearchTextChanged(string value) { ApplyFilter(); }

    private void ApplyFilter()
    {
        Products.Clear();
        var q = SearchText?.Trim() ?? "";
        var filtered = string.IsNullOrEmpty(q) ? _allProducts
            : _allProducts.Where(p => p.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                (p.Barcode != null && p.Barcode.Contains(q)) ||
                (p.Category != null && p.Category.Contains(q, StringComparison.OrdinalIgnoreCase))).ToList();
        foreach (var p in filtered) Products.Add(p);
    }

    [RelayCommand] private void ClearSearch() { SearchText = string.Empty; }
    [RelayCommand] private void ToggleAddPanel() { IsAddPanelOpen = !IsAddPanelOpen; }

    [RelayCommand]
    private void SaveProduct()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EditTitle)) return;
            using var db = DatabaseInitializer.CreateDbContext();
            db.Products.Add(new Product
            {
                Title = EditTitle, Barcode = string.IsNullOrWhiteSpace(EditBarcode) ? null : EditBarcode,
                Category = EditCategory, Unit = EditUnit, PurchasePrice = EditPurchasePrice,
                SalePrice = EditSalePrice, Stock = EditStock, MinStock = EditMinStock, WarehouseId = 1
            });
            db.SaveChanges();
            IsAddPanelOpen = false; ClearForm(); LoadProducts(); LoadCategories();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand]
    private void DeleteProduct(Product? p)
    {
        if (p == null) return;
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            var found = db.Products.Find(p.Id);
            if (found != null) { found.IsActive = false; db.SaveChanges(); }
            LoadProducts();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand] private void CancelEdit() { IsAddPanelOpen = false; ClearForm(); }
    private void ClearForm() { EditTitle = string.Empty; EditBarcode = string.Empty; EditCategory = string.Empty; EditPurchasePrice = 0; EditSalePrice = 0; EditStock = 0; EditMinStock = 0; EditUnit = UnitType.Number; }
}
