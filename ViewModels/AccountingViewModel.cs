using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class AccountingViewModel : ObservableObject
{
    private List<Account> _allAccounts = new();
    private List<JournalEntry> _allEntries = new();

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private bool _isAddPanelOpen;
    [ObservableProperty] private string _editDescription = string.Empty;
    [ObservableProperty] private decimal _editDebit;
    [ObservableProperty] private decimal _editCredit;
    [ObservableProperty] private Account? _editAccount;

    public ObservableCollection<Account> Accounts { get; } = new();
    public ObservableCollection<JournalEntry> Entries { get; } = new();
    public ObservableCollection<Account> AccountList { get; } = new();

    [ObservableProperty] private decimal _totalDebit;
    [ObservableProperty] private decimal _totalCredit;
    [ObservableProperty] private bool _isBalanced;

    public AccountingViewModel() { LoadData(); }

    public void LoadData()
    {
        using var db = DatabaseInitializer.CreateDbContext();
        _allAccounts = db.Accounts.AsNoTracking().Where(a => a.IsActive).OrderBy(a => a.Code).ToList();
        _allEntries = db.JournalEntries.AsNoTracking().Where(e => e.IsActive).OrderByDescending(e => e.EntryDate).ToList();

        AccountList.Clear();
        foreach (var a in _allAccounts) AccountList.Add(a);

        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value) { ApplyFilter(); }

    private void ApplyFilter()
    {
        Entries.Clear();
        var q = SearchText?.Trim() ?? "";
        var filtered = string.IsNullOrEmpty(q) ? _allEntries
            : _allEntries.Where(e =>
                e.EntryNumber.Contains(q) ||
                e.Description.Contains(q, StringComparison.OrdinalIgnoreCase))
            .ToList();
        foreach (var e in filtered) Entries.Add(e);
    }

    [RelayCommand]
    private void ClearSearch() { SearchText = string.Empty; }

    [RelayCommand]
    private void ToggleAddPanel() { IsAddPanelOpen = !IsAddPanelOpen; }

    [RelayCommand]
    private void AddLine()
    {
        if (EditAccount == null) return;
        if (EditDebit > 0) TotalDebit += EditDebit;
        if (EditCredit > 0) TotalCredit += EditCredit;
        IsBalanced = TotalDebit == TotalCredit && TotalDebit > 0;
    }

    [RelayCommand]
    private void SaveEntry()
    {
        if (TotalDebit == 0 || TotalCredit == 0 || !IsBalanced) return;
        using var db = DatabaseInitializer.CreateDbContext();
        var entry = new JournalEntry
        {
            EntryNumber = $"JE-{DateTime.Now:yyyyMMddHHmmss}",
            EntryDate = DateTime.Now,
            Description = EditDescription,
            Status = EntryStatus.Posted,
            UserId = AppSettings.CurrentUser?.Id ?? 1,
            TotalDebit = TotalDebit,
            TotalCredit = TotalCredit
        };
        db.JournalEntries.Add(entry);
        db.SaveChanges();
        IsAddPanelOpen = false;
        EditDescription = string.Empty; EditDebit = 0; EditCredit = 0;
        TotalDebit = 0; TotalCredit = 0; IsBalanced = false;
        LoadData();
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsAddPanelOpen = false;
        EditDescription = string.Empty; EditDebit = 0; EditCredit = 0;
        TotalDebit = 0; TotalCredit = 0; IsBalanced = false;
    }
}
