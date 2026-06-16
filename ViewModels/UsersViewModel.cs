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
    [ObservableProperty] private string _errorMessage = string.Empty;

    public ObservableCollection<User> Users { get; } = new();
    public UserRole[] Roles { get; } = Enum.GetValues<UserRole>();

    public UsersViewModel() { LoadUsers(); }

    public void LoadUsers()
    {
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            Users.Clear();
            foreach (var u in db.Users.AsNoTracking().Where(u => u.IsActive).OrderBy(u => u.Name).ToList())
                Users.Add(u);
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    public string GetRoleName(UserRole role) => role switch
    {
        UserRole.SuperAdmin => "مدیر ارشد", UserRole.Admin => "مدیر",
        UserRole.Cashier => "صندوقدار", UserRole.Broker => "مشاور",
        UserRole.Accountant => "حسابدار", _ => role.ToString()
    };

    [RelayCommand] private void ToggleAddPanel() { IsAddPanelOpen = !IsAddPanelOpen; }

    [RelayCommand]
    private void SaveUser()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EditName) || string.IsNullOrWhiteSpace(EditPin)) return;
            using var db = DatabaseInitializer.CreateDbContext();
            db.Users.Add(new User { Name = EditName, PinCodeHash = ComputeSha256(EditPin), Role = EditRole });
            db.SaveChanges();
            IsAddPanelOpen = false; EditName = string.Empty; EditPin = string.Empty; EditRole = UserRole.Cashier;
            LoadUsers();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand]
    private void DeleteUser(User? user)
    {
        if (user == null || AppSettings.CurrentUser?.Id == user.Id) return;
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            var u = db.Users.Find(user.Id);
            if (u != null) { u.IsActive = false; db.SaveChanges(); }
            LoadUsers();
        }
        catch (Exception ex) { ErrorMessage = "خطا: " + ex.Message; }
    }

    [RelayCommand] private void CancelEdit() { IsAddPanelOpen = false; EditName = string.Empty; EditPin = string.Empty; EditRole = UserRole.Cashier; }

    private static string ComputeSha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
