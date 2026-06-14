using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class CustomersViewModel : ObservableObject
{
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private bool _isAddPanelOpen;
    [ObservableProperty] private string _editName = string.Empty;
    [ObservableProperty] private string _editPhone = string.Empty;
    [ObservableProperty] private decimal _editCreditLimit;

    public ObservableCollection<Customer> Customers { get; } = new();

    public CustomersViewModel()
    {
        LoadCustomers();
    }

    public void LoadCustomers()
    {
        using var db = DatabaseInitializer.CreateDbContext();
        Customers.Clear();
        foreach (var c in db.Customers.AsNoTracking().ToList())
            Customers.Add(c);
    }

    [RelayCommand]
    private void ToggleAddPanel() => IsAddPanelOpen = !IsAddPanelOpen;

    [RelayCommand]
    private void SaveCustomer()
    {
        if (string.IsNullOrWhiteSpace(EditName)) return;
        using var db = DatabaseInitializer.CreateDbContext();
        var customer = new Customer
        {
            Name = EditName,
            Phone = EditPhone,
            CreditLimit = EditCreditLimit
        };
        db.Customers.Add(customer);
        db.SaveChanges();
        IsAddPanelOpen = false;
        ClearForm();
        LoadCustomers();
    }

    [RelayCommand]
    private void DeleteCustomer(Customer? customer)
    {
        if (customer == null) return;
        using var db = DatabaseInitializer.CreateDbContext();
        var c = db.Customers.Find(customer.Id);
        if (c != null)
        {
            c.IsActive = false;
            db.SaveChanges();
        }
        LoadCustomers();
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsAddPanelOpen = false;
        ClearForm();
    }

    private void ClearForm()
    {
        EditName = string.Empty;
        EditPhone = string.Empty;
        EditCreditLimit = 0;
    }
}
