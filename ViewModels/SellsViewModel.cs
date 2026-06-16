using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class SellsViewModel : ObservableObject
{
    private List<Sale> _allSales = new();

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;
    [ObservableProperty] private int _totalSalesCount;
    [ObservableProperty] private decimal _totalRevenue;
    [ObservableProperty] private decimal _totalProfit;
    [ObservableProperty] private int _returnedCount;

    public ObservableCollection<Sale> Sales { get; } = new();
    public ObservableCollection<SaleItem> SelectedSaleItems { get; } = new();

    [ObservableProperty] private Sale? _selectedSale;

    public SellsViewModel() { LoadData(); }

    public void LoadData()
    {
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            _allSales = db.Sales.AsNoTracking()
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .ToList();

            TotalSalesCount = _allSales.Count;
            TotalRevenue = _allSales.Sum(s => s.TotalAmount);
            TotalProfit = _allSales.Sum(s => s.TotalNetProfit);
            ReturnedCount = _allSales.Count(s => s.Status == SaleStatus.Returned);

            ApplyFilter();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    partial void OnSearchTextChanged(string value) { ApplyFilter(); }

    private void ApplyFilter()
    {
        Sales.Clear();
        var q = SearchText?.Trim() ?? "";
        var filtered = string.IsNullOrEmpty(q) ? _allSales
            : _allSales.Where(s =>
                s.InvoiceNumber.Contains(q) ||
                s.CreatedAt.ToString("yyyy/MM/dd").Contains(q)).ToList();
        foreach (var s in filtered) Sales.Add(s);
    }

    [RelayCommand]
    private void ClearSearch() { SearchText = string.Empty; }

    [RelayCommand]
    private void SelectSale(Sale? sale)
    {
        if (sale == null) return;
        SelectedSale = sale;
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            var items = db.SaleItems.AsNoTracking()
                .Where(i => i.SaleId == sale.Id && i.IsActive).ToList();
            SelectedSaleItems.Clear();
            foreach (var i in items) SelectedSaleItems.Add(i);
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand]
    private void ReturnSale(Sale? sale)
    {
        if (sale == null || sale.Status == SaleStatus.Returned) return;
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            var s = db.Sales.Find(sale.Id);
            if (s != null)
            {
                s.Status = SaleStatus.Returned;

                // Restore stock
                var items = db.SaleItems.Where(i => i.SaleId == s.Id && i.IsActive).ToList();
                foreach (var item in items)
                {
                    if (item.ProductId.HasValue)
                    {
                        var product = db.Products.Find(item.ProductId.Value);
                        if (product != null) product.Stock += item.Quantity;
                    }
                }

                db.SaveChanges();
            }
            LoadData();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }
}
