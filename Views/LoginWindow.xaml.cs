using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.Views;

public partial class LoginWindow : Window
{
    private List<User> _users = new();

    public LoginWindow()
    {
        try { InitializeComponent(); LoadUsers(); }
        catch (Exception ex) { MessageBox.Show("خطا: " + ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void LoadUsers()
    {
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            _users = db.Users.Where(u => u.IsActive).OrderBy(u => u.Name).ToList();
            UserCombo.ItemsSource = _users;
            if (_users.Count > 0) { UserCombo.SelectedIndex = 0; NoUsersText.Visibility = Visibility.Collapsed; }
            else { NoUsersText.Visibility = Visibility.Visible; LoginButton.IsEnabled = false; }
        }
        catch (Exception ex) { MessageBox.Show("خطا در بارگذاری کاربران: " + ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private void UserCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            if (UserCombo.SelectedItem is User user)
            {
                var roleNames = new Dictionary<UserRole, string>
                {
                    { UserRole.SuperAdmin, "مدیر ارشد" }, { UserRole.Admin, "مدیر" },
                    { UserRole.Cashier, "صندوقدار" }, { UserRole.Broker, "مشاور" },
                    { UserRole.Accountant, "حسابدار" }
                };
                RoleLabel.Text = roleNames.TryGetValue(user.Role, out var name) ? name : user.Role.ToString();
            }
        }
        catch { }
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (UserCombo.SelectedItem is not User user) { ErrorText.Text = "لطفاً کاربر را انتخاب کنید"; return; }
            if (string.IsNullOrWhiteSpace(PinBox.Password)) { ErrorText.Text = "لطفاً کد عبور را وارد کنید"; return; }

            var hash = ComputeSha256(PinBox.Password);
            if (user.PinCodeHash != hash) { ErrorText.Text = "کد عبور اشتباه است"; PinBox.Clear(); PinBox.Focus(); return; }

            AppSettings.CurrentUser = user;
            this.DialogResult = true;
            this.Close();
        }
        catch (Exception ex) { MessageBox.Show("خطا: " + ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error); }
    }

    private static string ComputeSha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
