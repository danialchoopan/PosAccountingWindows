using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class CustomersViewModel : ObservableObject
{
    private List<Customer> _allCustomers = new();

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private bool _isAddPanelOpen;
    [ObservableProperty] private string _editName = string.Empty;
    [ObservableProperty] private string _editPhone = string.Empty;
    [ObservableProperty] private decimal _editCreditLimit;
    [ObservableProperty] private string _errorMessage = string.Empty;

    public ObservableCollection<Customer> Customers { get; } = new();

    public CustomersViewModel() { LoadCustomers(); }

    public void LoadCustomers()
    {
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            _allCustomers = db.Customers.AsNoTracking().Where(c => c.IsActive).OrderBy(c => c.Name).ToList();
            ApplyFilter();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    partial void OnSearchTextChanged(string value) { ApplyFilter(); }

    private void ApplyFilter()
    {
        Customers.Clear();
        var q = SearchText?.Trim() ?? "";
        var filtered = string.IsNullOrEmpty(q) ? _allCustomers
            : _allCustomers.Where(c => c.Name.Contains(q, StringComparison.OrdinalIgnoreCase) || c.Phone.Contains(q)).ToList();
        foreach (var c in filtered) Customers.Add(c);
    }

    [RelayCommand] private void ClearSearch() { SearchText = string.Empty; }
    [RelayCommand] private void ToggleAddPanel() { IsAddPanelOpen = !IsAddPanelOpen; }

    [RelayCommand]
    private void SaveCustomer()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EditName)) return;
            using var db = DatabaseInitializer.CreateDbContext();
            db.Customers.Add(new Customer { Name = EditName, Phone = EditPhone, CreditLimit = EditCreditLimit });
            db.SaveChanges();
            IsAddPanelOpen = false; EditName = string.Empty; EditPhone = string.Empty; EditCreditLimit = 0;
            LoadCustomers();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand]
    private void DeleteCustomer(Customer? c)
    {
        if (c == null) return;
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            var found = db.Customers.Find(c.Id);
            if (found != null) { found.IsActive = false; db.SaveChanges(); }
            LoadCustomers();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand] private void CancelEdit() { IsAddPanelOpen = false; EditName = string.Empty; EditPhone = string.Empty; EditCreditLimit = 0; }
}
