using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;
using PosAccountingApp.Services;

namespace PosAccountingApp.ViewModels;

public partial class AccountingViewModel : ObservableObject
{
    private readonly AttachmentService _attachmentService = new();
    private List<Account> _allAccounts = new();
    private List<JournalEntry> _allEntries = new();

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private bool _isAddPanelOpen;
    [ObservableProperty] private string _editDescription = string.Empty;
    [ObservableProperty] private decimal _editDebit;
    [ObservableProperty] private decimal _editCredit;
    [ObservableProperty] private Account? _editAccount;
    [ObservableProperty] private string _errorMessage = string.Empty;
    [ObservableProperty] private string _attachmentCount = string.Empty;

    public ObservableCollection<Account> Accounts { get; } = new();
    public ObservableCollection<JournalEntry> Entries { get; } = new();
    public ObservableCollection<Attachment> Attachments { get; } = new();

    [ObservableProperty] private decimal _totalDebit;
    [ObservableProperty] private decimal _totalCredit;
    [ObservableProperty] private bool _isBalanced;

    public AccountingViewModel() { LoadData(); }

    public void LoadData()
    {
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            _allAccounts = db.Accounts.AsNoTracking().Where(a => a.IsActive).OrderBy(a => a.Code).ToList();
            _allEntries = db.JournalEntries.AsNoTracking().Where(e => e.IsActive).OrderByDescending(e => e.EntryDate).ToList();

            AccountList.Clear();
            foreach (var a in _allAccounts) AccountList.Add(a);

            ApplyFilter();
            ErrorMessage = string.Empty;
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    partial void OnSearchTextChanged(string value) { ApplyFilter(); }

    private void ApplyFilter()
    {
        Entries.Clear();
        var q = SearchText?.Trim() ?? "";
        var filtered = string.IsNullOrEmpty(q) ? _allEntries
            : _allEntries.Where(e => e.EntryNumber.Contains(q) || e.Description.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
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
        try
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
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsAddPanelOpen = false;
        EditDescription = string.Empty; EditDebit = 0; EditCredit = 0;
        TotalDebit = 0; TotalCredit = 0; IsBalanced = false;
    }

    public ObservableCollection<Account> AccountList { get; } = new();
}
