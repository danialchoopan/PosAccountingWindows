using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty] private string _businessName = "";
    [ObservableProperty] private string _businessPhone = string.Empty;
    [ObservableProperty] private string _businessAddress = string.Empty;
    [ObservableProperty] private decimal _vatPercentage = 10;
    [ObservableProperty] private decimal _commissionPercentage = 2;
    [ObservableProperty] private RoundingMode _roundingMode = RoundingMode.Off;
    [ObservableProperty] private int _selectedThemeIndex = 0;
    [ObservableProperty] private double _fontSize = 14;
    [ObservableProperty] private bool _isHighContrast;
    [ObservableProperty] private string _currencySymbol = "ريال";
    [ObservableProperty] private string _receiptFooter = "با تشکر از خرید شما";
    [ObservableProperty] private string _receiptHeader = "فاکتور فروش";
    [ObservableProperty] private string _invoicePrefix = "INV";
    [ObservableProperty] private int _addModeIndex = 2;
    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private int _selectedProfileIndex = 0;

    public string[] ThemeNames { get; } = ["اقیانوس آبی", "سبز زمردی", "بنفش سلطنتی", "غروب نارنجی", "نیمه‌شب تاریک"];
    public string[] ProfileNames { get; } = ["سوپرمارکت", "پوشاک", "املاک", "خودرو", "مصالح"];
    public double[] FontSizes { get; } = [10, 12, 14, 16, 18, 20];
    public RoundingMode[] RoundingModes { get; } = Enum.GetValues<RoundingMode>();

    public SettingsViewModel() { LoadSettings(); }

    public void LoadSettings()
    {
        try
        {
            var s = AppSettings.Load();
            BusinessName = s.ShopName;
            BusinessPhone = s.Phone;
            BusinessAddress = s.Address;
            VatPercentage = s.VatPercentage;
            CommissionPercentage = s.CommissionPercentage;
            SelectedThemeIndex = GetThemeIndex(s.SelectedTheme);
            FontSize = s.FontSize > 0 ? s.FontSize : 14;
            IsHighContrast = s.IsHighContrast;
            CurrencySymbol = s.CurrencySymbol ?? "ريال";
            ReceiptFooter = s.ReceiptFooter ?? "با تشکر از خرید شما";
            ReceiptHeader = s.ReceiptHeader ?? "فاکتور فروش";
            InvoicePrefix = s.InvoicePrefix ?? "INV";
            AddModeIndex = s.AddModeIndex;
            SelectedProfileIndex = s.SelectedProfileIndex;
        }
        catch { }
    }

    private static int GetThemeIndex(string theme) => theme switch
    {
        "EmeraldGreen" => 1, "RoyalPurple" => 2,
        "SunsetOrange" => 3, "MidnightDark" => 4,
        _ => 0
    };

    [RelayCommand]
    private void SaveSettings()
    {
        try
        {
            // Apply theme immediately
            var theme = (AppTheme)SelectedThemeIndex;
            ThemeManager.ApplyTheme(theme);

            // Apply font size immediately
            var app = Application.Current;
            if (app != null)
                app.Resources["AppFontSize"] = FontSize;

            // Apply high contrast immediately
            if (IsHighContrast)
            {
                if (app != null)
                {
                    app.Resources["TextPrimaryBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
                    app.Resources["TextSecondaryBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkGray);
                    app.Resources["BgBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                    app.Resources["CardBgBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                }
            }
            else
            {
                ThemeManager.ApplyTheme(theme);
            }

            // Save to file
            var s = AppSettings.Load();
            s.ShopName = BusinessName;
            s.Phone = BusinessPhone;
            s.Address = BusinessAddress;
            s.VatPercentage = VatPercentage;
            s.CommissionPercentage = CommissionPercentage;
            s.SelectedTheme = theme.ToString();
            s.FontSize = FontSize;
            s.IsHighContrast = IsHighContrast;
            s.CurrencySymbol = CurrencySymbol;
            s.ReceiptFooter = ReceiptFooter;
            s.ReceiptHeader = ReceiptHeader;
            s.InvoicePrefix = InvoicePrefix;
            s.AddModeIndex = AddModeIndex;
            s.SelectedProfileIndex = SelectedProfileIndex;
            s.Save();

            StatusMessage = "تنظیمات ذخیره و اعمال شد";
        }
        catch (Exception ex)
        {
            StatusMessage = "خطا: " + ex.Message;
        }
    }

    [RelayCommand]
    private void BackupDatabase()
    {
        var dbPath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PosAccountingApp", "pos_data.db");
        var backupPath = System.IO.Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            $"pos_backup_{DateTime.Now:yyyyMMdd_HHmmss}.db");
        if (System.IO.File.Exists(dbPath))
        {
            System.IO.File.Copy(dbPath, backupPath, true);
            StatusMessage = "پشتیبان ذخیره شد در دسکتاپ";
        }
    }

    [RelayCommand]
    private void RestoreDatabase() { StatusMessage = "بازیابی"; }

    [RelayCommand]
    private void RunSeedData()
    {
        try
        {
            Data.SeedData.Seed();
            StatusMessage = "داده نمونه با موفقیت ایجاد شد";
        }
        catch (Exception ex)
        {
            StatusMessage = "خطا: " + ex.Message;
        }
    }
}
