using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class GlobalSearchViewModel : ObservableObject
{
    [ObservableProperty] private string _searchText = string.Empty;

    public ObservableCollection<SearchResultItem> Results { get; } = new();

    partial void OnSearchTextChanged(string value)
    {
        PerformSearch(value);
    }

    public void PerformSearch(string query)
    {
        Results.Clear();
        var q = query?.Trim() ?? "";
        if (string.IsNullOrEmpty(q)) return;

        try
        {
            using var db = DatabaseInitializer.CreateDbContext();

            // Search products
            var products = db.Products.AsNoTracking()
                .Where(p => p.IsActive && (p.Title.Contains(q) || (p.Barcode != null && p.Barcode.Contains(q))))
                .Take(10).ToList();
            foreach (var p in products)
                Results.Add(new SearchResultItem { Type = "کالا", Name = p.Title, Detail = $"بارکد: {p.Barcode ?? "-"} | موجودی: {p.Stock}", Page = "Products" });

            // Search customers
            var customers = db.Customers.AsNoTracking()
                .Where(c => c.IsActive && (c.Name.Contains(q) || c.Phone.Contains(q)))
                .Take(10).ToList();
            foreach (var c in customers)
                Results.Add(new SearchResultItem { Type = "مشتری", Name = c.Name, Detail = $"تلفن: {c.Phone} | موجودی: {c.Balance:N0}", Page = "Customers" });

            // Search cheques
            var cheques = db.Cheques.AsNoTracking()
                .Where(c => c.IsActive && (c.ChequeNumber.Contains(q) || c.PayerName.Contains(q) || c.BankName.Contains(q)))
                .Take(10).ToList();
            foreach (var c in cheques)
                Results.Add(new SearchResultItem { Type = "چک", Name = $"{c.ChequeNumber}", Detail = $"{c.BankName} | {c.Amount:N0} ریال | {c.PayerName}", Page = "Cheques" });

            // Search users
            var users = db.Users.AsNoTracking()
                .Where(u => u.IsActive && u.Name.Contains(q))
                .Take(5).ToList();
            foreach (var u in users)
                Results.Add(new SearchResultItem { Type = "کاربر", Name = u.Name, Detail = $"نقش: {u.Role}", Page = "Users" });
        }
        catch { }
    }
}

public class SearchResultItem
{
    public string Type { get; set; } = "";
    public string Name { get; set; } = "";
    public string Detail { get; set; } = "";
    public string Page { get; set; } = "";
}
