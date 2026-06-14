using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class ExpensesViewModel : ObservableObject
{
    [ObservableProperty] private bool _isAddPanelOpen;
    [ObservableProperty] private string _editDescription = string.Empty;
    [ObservableProperty] private decimal _editAmount;
    [ObservableProperty] private ExpenseCategory _editCategory = ExpenseCategory.Other;
    [ObservableProperty] private DateTime _editDate = DateTime.Now;

    public ObservableCollection<Expense> Expenses { get; } = new();
    public ExpenseCategory[] Categories { get; } = Enum.GetValues<ExpenseCategory>();

    public ExpensesViewModel()
    {
        LoadExpenses();
    }

    public void LoadExpenses()
    {
        using var db = DatabaseInitializer.CreateDbContext();
        Expenses.Clear();
        foreach (var e in db.Expenses.AsNoTracking().ToList())
            Expenses.Add(e);
    }

    [RelayCommand]
    private void ToggleAddPanel() => IsAddPanelOpen = !IsAddPanelOpen;

    [RelayCommand]
    private void SaveExpense()
    {
        if (string.IsNullOrWhiteSpace(EditDescription) || EditAmount <= 0) return;
        using var db = DatabaseInitializer.CreateDbContext();
        var expense = new Expense
        {
            Description = EditDescription,
            Amount = EditAmount,
            Category = EditCategory,
            Date = EditDate
        };
        db.Expenses.Add(expense);
        db.SaveChanges();
        IsAddPanelOpen = false;
        LoadExpenses();
    }

    [RelayCommand]
    private void CancelEdit() => IsAddPanelOpen = false;
}
