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
    [ObservableProperty] private ProductCategory? _editParentCategory;
    [ObservableProperty] private string _errorMessage = string.Empty;

    public ObservableCollection<ProductCategory> Categories { get; } = new();
    public ObservableCollection<ProductCategory> ParentCategories { get; } = new();

    public string[] AvailableIcons { get; } =
    [
        "\uE74C", "\uE71C", "\uE77B", "\uE7F3", "\uE8D8",
        "\uE80F", "\uE74D", "\uE71E", "\uE774", "\uE719",
        "\uE736", "\uE740", "\uE78B", "\uE787", "\uE72D",
        "\uE7BA", "\uE9D9", "\uE74E", "\uE7C3", "\uE716"
    ];

    public CategoriesViewModel() { LoadCategories(); }

    public void LoadCategories()
    {
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            _allCategories = db.Categories.AsNoTracking().Where(c => c.IsActive).OrderBy(c => c.SortOrder).ThenBy(c => c.Name).ToList();
            foreach (var cat in _allCategories)
            {
                if (cat.ParentCategoryId.HasValue)
                {
                    var parent = _allCategories.FirstOrDefault(p => p.Id == cat.ParentCategoryId);
                    cat.FullName = parent != null ? $"{parent.Name} / {cat.Name}" : cat.Name;
                }
                else cat.FullName = cat.Name;
            }
            ParentCategories.Clear();
            foreach (var c in _allCategories.Where(c => !c.ParentCategoryId.HasValue)) ParentCategories.Add(c);
            ApplyFilter();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    partial void OnSearchTextChanged(string value) { ApplyFilter(); }

    private void ApplyFilter()
    {
        Categories.Clear();
        var q = SearchText?.Trim() ?? "";
        var filtered = string.IsNullOrEmpty(q) ? _allCategories
            : _allCategories.Where(c => c.Name.Contains(q, StringComparison.OrdinalIgnoreCase) || (c.FullName != null && c.FullName.Contains(q, StringComparison.OrdinalIgnoreCase))).ToList();
        foreach (var c in filtered) Categories.Add(c);
    }

    [RelayCommand] private void ClearSearch() { SearchText = string.Empty; }
    [RelayCommand] private void ToggleAddPanel() { IsAddPanelOpen = !IsAddPanelOpen; }

    [RelayCommand]
    private void SaveCategory()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EditName)) return;
            using var db = DatabaseInitializer.CreateDbContext();
            db.Categories.Add(new ProductCategory { Name = EditName, Description = EditDescription, Icon = EditIcon, SortOrder = EditSortOrder, ParentCategoryId = EditParentCategory?.Id });
            db.SaveChanges();
            IsAddPanelOpen = false; LoadCategories();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand]
    private void DeleteCategory(ProductCategory? cat)
    {
        if (cat == null) return;
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            var found = db.Categories.Find(cat.Id);
            if (found != null) { found.IsActive = false; db.SaveChanges(); }
            LoadCategories();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand] private void CancelEdit() { IsAddPanelOpen = false; EditName = string.Empty; EditDescription = string.Empty; EditIcon = "\uE74C"; EditSortOrder = 0; EditParentCategory = null; }
}
