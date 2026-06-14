using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.Views;

public partial class SetupWindow : Window
{
    public SetupWindow()
    {
        InitializeComponent();
        BusinessTypeCombo.SelectedIndex = 0;
    }

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ShopNameBox.Text))
        {
            ErrorText.Text = "لطفاً نام فروشگاه را وارد کنید";
            return;
        }
        if (string.IsNullOrWhiteSpace(PinBox.Text) || PinBox.Text.Length != 4 || !PinBox.Text.All(char.IsDigit))
        {
            ErrorText.Text = "کد عبور باید دقیقاً ۴ رقم باشد";
            return;
        }
        if (BusinessTypeCombo.SelectedItem is not ComboBoxItem selectedType)
        {
            ErrorText.Text = "لطفاً نوع کسب‌وکار را انتخاب کنید";
            return;
        }

        ErrorText.Text = "در حال راه‌اندازی...";

        try
        {
            DatabaseInitializer.Initialize();
            using var db = DatabaseInitializer.CreateDbContext();

            var admin = db.Users.First(u => u.Id == 1);
            admin.PinCodeHash = ComputeSha256(PinBox.Text);
            admin.Name = "مدیر";

            db.Warehouses.Add(new Warehouse { Name = "انبار اصلی", Location = "محل فروشگاه" });
            db.SaveChanges();

            var settingsDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PosAccountingApp");
            Directory.CreateDirectory(settingsDir);

            var settings = new AppSettings
            {
                ShopName = ShopNameBox.Text,
                BusinessType = selectedType.Tag?.ToString() ?? "Supermarket",
                Phone = PhoneBox.Text ?? "",
                Address = AddressBox.Text ?? "",
                VatPercentage = decimal.TryParse(VatBox.Text, out var vat) ? vat : 10,
                IsSetupComplete = true
            };

            var json = System.Text.Json.JsonSerializer.Serialize(settings,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Combine(settingsDir, "settings.json"), json);

            // Close dialog with success
            this.DialogResult = true;
            this.Close();
        }
        catch (Exception ex)
        {
            ErrorText.Text = "خطا: " + ex.Message;
        }
    }

    private static string ComputeSha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
