using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class InventoryViewModel : ObservableObject
{
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;
    [ObservableProperty] private int _totalCount;
    [ObservableProperty] private int _lowStockCount;
    [ObservableProperty] private int _emptyStockCount;
    [ObservableProperty] private decimal _totalValue;

    public ObservableCollection<Product> AllProducts { get; } = new();
    public ObservableCollection<Product> LowStockProducts { get; } = new();
    public ObservableCollection<Product> EmptyStockProducts { get; } = new();

    public InventoryViewModel() { LoadData(); }

    public void LoadData()
    {
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            var products = db.Products.AsNoTracking().Where(p => p.IsActive).OrderBy(p => p.Title).ToList();

            AllProducts.Clear();
            LowStockProducts.Clear();
            EmptyStockProducts.Clear();

            foreach (var p in products)
            {
                AllProducts.Add(p);
                if (p.Stock <= 0)
                    EmptyStockProducts.Add(p);
                else if (p.Stock <= p.MinStock)
                    LowStockProducts.Add(p);
            }

            TotalCount = AllProducts.Count;
            LowStockCount = LowStockProducts.Count;
            EmptyStockCount = EmptyStockProducts.Count;
            TotalValue = AllProducts.Sum(p => p.Stock * p.SalePrice);
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand]
    private void Refresh() => LoadData();
}
