using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PosAccountingApp.Models;
using PosAccountingApp.Services;

namespace PosAccountingApp.ViewModels;

public partial class SupplierViewModel : ObservableObject
{
    private readonly SupplierService _supplierService = new();
    private List<Supplier> _allSuppliers = new();

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private bool _isAddPanelOpen;
    [ObservableProperty] private bool _isPaymentPanelOpen;
    [ObservableProperty] private Supplier? _selectedSupplier;
    [ObservableProperty] private string _errorMessage = string.Empty;

    [ObservableProperty] private string _editName = string.Empty;
    [ObservableProperty] private string _editPhone = string.Empty;
    [ObservableProperty] private string _editAddress = string.Empty;
    [ObservableProperty] private string _editContactPerson = string.Empty;

    [ObservableProperty] private decimal _paymentAmount;
    [ObservableProperty] private string _paymentDescription = string.Empty;

    public ObservableCollection<Supplier> Suppliers { get; } = new();
    public ObservableCollection<SupplierLedgerEntry> LedgerEntries { get; } = new();

    [ObservableProperty] private decimal _totalDebt;

    public SupplierViewModel() { LoadSuppliers(); }

    public void LoadSuppliers()
    {
        try
        {
            _allSuppliers = _supplierService.GetAllSuppliers();
            TotalDebt = _supplierService.GetTotalSupplierDebt();
            ApplyFilter();
            ErrorMessage = string.Empty;
        }
        catch (Exception ex)
        {
            ErrorMessage = "خطا در بارگذاری تامین‌کنندگان: " + ex.Message;
        }
    }

    partial void OnSearchTextChanged(string value) { ApplyFilter(); }

    private void ApplyFilter()
    {
        Suppliers.Clear();
        var q = SearchText?.Trim() ?? "";
        var filtered = string.IsNullOrEmpty(q) ? _allSuppliers
            : _allSuppliers.Where(s =>
                s.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                s.Phone.Contains(q)).ToList();
        foreach (var s in filtered) Suppliers.Add(s);
    }

    [RelayCommand]
    private void ClearSearch() { SearchText = string.Empty; }

    [RelayCommand]
    private void ToggleAddPanel() { IsAddPanelOpen = !IsAddPanelOpen; IsPaymentPanelOpen = false; }

    [RelayCommand]
    private void SaveSupplier()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EditName)) return;
            _supplierService.CreateSupplier(EditName, EditPhone, EditAddress, EditContactPerson);
            IsAddPanelOpen = false;
            ClearForm();
            LoadSuppliers();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand]
    private void SelectSupplier(Supplier? supplier)
    {
        if (supplier == null) return;
        SelectedSupplier = supplier;
        try
        {
            var ledger = _supplierService.GetSupplierLedger(supplier.Id);
            LedgerEntries.Clear();
            foreach (var e in ledger) LedgerEntries.Add(e);
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand]
    private void OpenPaymentPanel()
    {
        if (SelectedSupplier == null) return;
        IsPaymentPanelOpen = true;
        PaymentAmount = 0;
        PaymentDescription = string.Empty;
    }

    [RelayCommand]
    private void RecordPayment()
    {
        try
        {
            if (SelectedSupplier == null || PaymentAmount <= 0) return;
            _supplierService.RecordPayment(SelectedSupplier.Id, PaymentAmount,
                string.IsNullOrEmpty(PaymentDescription) ? "پرداخت" : PaymentDescription);
            IsPaymentPanelOpen = false;
            LoadSuppliers();
            SelectSupplier(SelectedSupplier);
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand]
    private void DeleteSupplier(Supplier? supplier)
    {
        if (supplier == null) return;
        try
        {
            _supplierService.DeleteSupplier(supplier.Id);
            if (SelectedSupplier?.Id == supplier.Id) SelectedSupplier = null;
            LoadSuppliers();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand]
    private void CancelEdit() { IsAddPanelOpen = false; ClearForm(); }

    [RelayCommand]
    private void CancelPayment() { IsPaymentPanelOpen = false; }

    private void ClearForm()
    {
        EditName = string.Empty; EditPhone = string.Empty;
        EditAddress = string.Empty; EditContactPerson = string.Empty;
    }
}
