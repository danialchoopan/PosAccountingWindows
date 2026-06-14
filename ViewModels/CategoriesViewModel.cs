using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class CategoriesViewModel : ObservableObject
{
    private List<ProductCategory> _allCategories = new();

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private bool _isAddPanelOpen;
    [ObservableProperty] private string _editName = string.Empty;
    [ObservableProperty] private string _editDescription = string.Empty;
    [ObservableProperty] private string _editIcon = "\uE74C";
    [ObservableProperty] private int _editSortOrder;

    public ObservableCollection<ProductCategory> Categories { get; } = new();

    // Common icons for categories
    public string[] AvailableIcons { get; } =
    [
        "\uE74C", "\uE71C", "\uE77B", "\uE7F3", "\uE8D8",
        "\uE80F", "\uE74D", "\uE71E", "\uE774", "\uE719",
        "\uE736", "\uE740", "\uE78B", "\uE787", "\uE72D"
    ];

    public CategoriesViewModel() { LoadCategories(); }

    public void LoadCategories()
    {
        using var db = DatabaseInitializer.CreateDbContext();
        _allCategories = db.Categories.AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToList();
        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value) { ApplyFilter(); }

    private void ApplyFilter()
    {
        Categories.Clear();
        var q = SearchText?.Trim() ?? "";
        var filtered = string.IsNullOrEmpty(q) ? _allCategories
            : _allCategories.Where(c =>
                c.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                (c.Description != null && c.Description.Contains(q, StringComparison.OrdinalIgnoreCase)))
            .ToList();
        foreach (var c in filtered) Categories.Add(c);
    }

    [RelayCommand]
    private void ClearSearch() { SearchText = string.Empty; }

    [RelayCommand]
    private void ToggleAddPanel() => IsAddPanelOpen = !IsAddPanelOpen;

    [RelayCommand]
    private void SaveCategory()
    {
        if (string.IsNullOrWhiteSpace(EditName)) return;
        using var db = DatabaseInitializer.CreateDbContext();
        db.Categories.Add(new ProductCategory
        {
            Name = EditName,
            Description = EditDescription,
            Icon = EditIcon,
            SortOrder = EditSortOrder
        });
        db.SaveChanges();
        IsAddPanelOpen = false;
        EditName = string.Empty; EditDescription = string.Empty;
        EditIcon = "\uE74C"; EditSortOrder = 0;
        LoadCategories();
    }

    [RelayCommand]
    private void DeleteCategory(ProductCategory? cat)
    {
        if (cat == null) return;
        using var db = DatabaseInitializer.CreateDbContext();
        var found = db.Categories.Find(cat.Id);
        if (found != null) { found.IsActive = false; db.SaveChanges(); }
        LoadCategories();
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsAddPanelOpen = false;
        EditName = string.Empty; EditDescription = string.Empty;
        EditIcon = "\uE74C"; EditSortOrder = 0;
    }
}
