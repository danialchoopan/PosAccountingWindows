using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class UsersViewModel : ObservableObject
{
    [ObservableProperty] private bool _isAddPanelOpen;
    [ObservableProperty] private string _editName = string.Empty;
    [ObservableProperty] private string _editPin = string.Empty;
    [ObservableProperty] private UserRole _editRole = UserRole.Cashier;

    public ObservableCollection<User> Users { get; } = new();
    public UserRole[] Roles { get; } = Enum.GetValues<UserRole>();

    private static readonly Dictionary<UserRole, string> RoleNames = new()
    {
        { UserRole.SuperAdmin, "مدیر ارشد" },
        { UserRole.Admin, "مدیر" },
        { UserRole.Cashier, "صندوقدار" },
        { UserRole.Broker, "مشاور" },
        { UserRole.Accountant, "حسابدار" }
    };

    public UsersViewModel()
    {
        LoadUsers();
    }

    public void LoadUsers()
    {
        using var db = DatabaseInitializer.CreateDbContext();
        Users.Clear();
        foreach (var u in db.Users.AsNoTracking().Where(u => u.IsActive).OrderBy(u => u.Name).ToList())
            Users.Add(u);
    }

    public string GetRoleName(UserRole role)
    {
        return RoleNames.TryGetValue(role, out var name) ? name : role.ToString();
    }

    [RelayCommand]
    private void ToggleAddPanel() => IsAddPanelOpen = !IsAddPanelOpen;

    [RelayCommand]
    private void SaveUser()
    {
        if (string.IsNullOrWhiteSpace(EditName) || string.IsNullOrWhiteSpace(EditPin))
            return;

        using var db = DatabaseInitializer.CreateDbContext();

        var user = new User
        {
            Name = EditName,
            PinCodeHash = ComputeSha256(EditPin),
            Role = EditRole
        };

        db.Users.Add(user);
        db.SaveChanges();

        IsAddPanelOpen = false;
        EditName = string.Empty;
        EditPin = string.Empty;
        EditRole = UserRole.Cashier;
        LoadUsers();
    }

    [RelayCommand]
    private void DeleteUser(User? user)
    {
        if (user == null) return;

        // Don't delete yourself
        if (AppSettings.CurrentUser != null && user.Id == AppSettings.CurrentUser.Id)
            return;

        using var db = DatabaseInitializer.CreateDbContext();
        var u = db.Users.Find(user.Id);
        if (u != null)
        {
            u.IsActive = false;
            db.SaveChanges();
        }
        LoadUsers();
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsAddPanelOpen = false;
        EditName = string.Empty;
        EditPin = string.Empty;
        EditRole = UserRole.Cashier;
    }

    private static string ComputeSha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
