using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class ExpensesViewModel : ObservableObject
{
    private List<Expense> _allExpenses = new();

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private bool _isAddPanelOpen;
    [ObservableProperty] private string _editDescription = string.Empty;
    [ObservableProperty] private decimal _editAmount;
    [ObservableProperty] private ExpenseCategory _editCategory = ExpenseCategory.Other;
    [ObservableProperty] private DateTime _editDate = DateTime.Now;
    [ObservableProperty] private string _errorMessage = string.Empty;

    public ObservableCollection<Expense> Expenses { get; } = new();
    public ExpenseCategory[] Categories { get; } = Enum.GetValues<ExpenseCategory>();

    public ExpensesViewModel() { LoadExpenses(); }

    public void LoadExpenses()
    {
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            _allExpenses = db.Expenses.AsNoTracking().Where(e => e.IsActive).OrderByDescending(e => e.Date).ToList();
            ApplyFilter();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    partial void OnSearchTextChanged(string value) { ApplyFilter(); }

    private void ApplyFilter()
    {
        Expenses.Clear();
        var q = SearchText?.Trim() ?? "";
        var filtered = string.IsNullOrEmpty(q) ? _allExpenses
            : _allExpenses.Where(e => e.Description.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        foreach (var e in filtered) Expenses.Add(e);
    }

    [RelayCommand] private void ClearSearch() { SearchText = string.Empty; }
    [RelayCommand] private void ToggleAddPanel() { IsAddPanelOpen = !IsAddPanelOpen; }

    [RelayCommand]
    private void SaveExpense()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EditDescription) || EditAmount <= 0) return;
            using var db = DatabaseInitializer.CreateDbContext();
            db.Expenses.Add(new Expense { Description = EditDescription, Amount = EditAmount, Category = EditCategory, Date = EditDate });
            db.SaveChanges();
            IsAddPanelOpen = false; LoadExpenses();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand] private void CancelEdit() => IsAddPanelOpen = false;
}
