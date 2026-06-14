using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class ChequesViewModel : ObservableObject
{
    private List<Cheque> _allCheques = new();

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private bool _isAddPanelOpen;
    [ObservableProperty] private string _editChequeNumber = string.Empty;
    [ObservableProperty] private string _editBankName = string.Empty;
    [ObservableProperty] private string _editBranch = string.Empty;
    [ObservableProperty] private decimal _editAmount;
    [ObservableProperty] private DateTime _editDueDate = DateTime.Now;
    [ObservableProperty] private string _editPayerName = string.Empty;
    [ObservableProperty] private string _editReceiverName = string.Empty;
    [ObservableProperty] private ChequeType _editType = ChequeType.Receivable;

    public ObservableCollection<Cheque> Cheques { get; } = new();

    public ChequesViewModel() { LoadCheques(); }

    public void LoadCheques()
    {
        using var db = DatabaseInitializer.CreateDbContext();
        _allCheques = db.Cheques.AsNoTracking().Where(c => c.IsActive).OrderByDescending(c => c.CreatedAt).ToList();
        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value) { ApplyFilter(); }

    private void ApplyFilter()
    {
        Cheques.Clear();
        var q = SearchText?.Trim() ?? "";
        var filtered = string.IsNullOrEmpty(q) ? _allCheques
            : _allCheques.Where(c =>
                c.ChequeNumber.Contains(q) ||
                c.BankName.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                c.PayerName.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                c.ReceiverName.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var c in filtered) Cheques.Add(c);
    }

    [RelayCommand]
    private void ClearSearch() { SearchText = string.Empty; }

    [RelayCommand]
    private void ToggleAddPanel() => IsAddPanelOpen = !IsAddPanelOpen;

    [RelayCommand]
    private void SaveCheque()
    {
        if (string.IsNullOrWhiteSpace(EditChequeNumber)) return;
        using var db = DatabaseInitializer.CreateDbContext();
        db.Cheques.Add(new Cheque
        {
            ChequeNumber = EditChequeNumber, BankName = EditBankName, Branch = EditBranch,
            Amount = EditAmount, DueDate = EditDueDate, PayerName = EditPayerName,
            ReceiverName = EditReceiverName, Type = EditType
        });
        db.SaveChanges();
        IsAddPanelOpen = false;
        LoadCheques();
    }

    [RelayCommand]
    private void CancelEdit() => IsAddPanelOpen = false;
}
