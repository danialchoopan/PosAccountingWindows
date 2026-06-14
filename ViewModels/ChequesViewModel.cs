using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class ChequesViewModel : ObservableObject
{
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

    public ChequesViewModel()
    {
        LoadCheques();
    }

    public void LoadCheques()
    {
        using var db = DatabaseInitializer.CreateDbContext();
        Cheques.Clear();
        foreach (var c in db.Cheques.AsNoTracking().ToList())
            Cheques.Add(c);
    }

    [RelayCommand]
    private void ToggleAddPanel() => IsAddPanelOpen = !IsAddPanelOpen;

    [RelayCommand]
    private void SaveCheque()
    {
        if (string.IsNullOrWhiteSpace(EditChequeNumber)) return;
        using var db = DatabaseInitializer.CreateDbContext();
        var cheque = new Cheque
        {
            ChequeNumber = EditChequeNumber,
            BankName = EditBankName,
            Branch = EditBranch,
            Amount = EditAmount,
            DueDate = EditDueDate,
            PayerName = EditPayerName,
            ReceiverName = EditReceiverName,
            Type = EditType
        };
        db.Cheques.Add(cheque);
        db.SaveChanges();
        IsAddPanelOpen = false;
        LoadCheques();
    }

    [RelayCommand]
    private void CancelEdit() => IsAddPanelOpen = false;
}
