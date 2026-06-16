using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty] private BusinessProfile _selectedProfile = BusinessProfile.Supermarket;
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

    public string[] ThemeNames { get; } = ["اقیانوس آبی", "سبز زمردی", "بنفش سلطنتی", "غروب نارنجی", "نیمه‌شب تاریک"];
    public double[] FontSizes { get; } = [10, 12, 14, 16, 18, 20];
    public BusinessProfile[] Profiles { get; } = Enum.GetValues<BusinessProfile>();
    public RoundingMode[] RoundingModes { get; } = Enum.GetValues<RoundingMode>();

    public SettingsViewModel()
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
    }

    private static int GetThemeIndex(string theme) => theme switch
    {
        "EmeraldGreen" => 1, "RoyalPurple" => 2,
        "SunsetOrange" => 3, "MidnightDark" => 4,
        _ => 0
    };

    [RelayCommand]
    private void ApplyTheme()
    {
        var theme = (AppTheme)SelectedThemeIndex;
        ThemeManager.ApplyTheme(theme);
        var s = AppSettings.Load();
        s.SelectedTheme = theme.ToString();
        s.Save();
    }

    [RelayCommand]
    private void ApplyFontSize()
    {
        var app = Application.Current;
        if (app != null)
        {
            app.Resources["AppFontSize"] = FontSize;
            app.Resources["AppFontLarge"] = FontSize + 4;
            app.Resources["AppFontSmall"] = FontSize - 2;
        }
        var s = AppSettings.Load();
        s.FontSize = FontSize;
        s.Save();
    }

    [RelayCommand]
    private void ToggleHighContrast()
    {
        IsHighContrast = !IsHighContrast;
        var app = Application.Current;
        if (app == null) return;

        if (IsHighContrast)
        {
            app.Resources["TextPrimaryBrush"] = new SolidColorBrush(System.Windows.Media.Colors.Black);
            app.Resources["TextSecondaryBrush"] = new SolidColorBrush(System.Windows.Media.Colors.DarkGray);
            app.Resources["BgBrush"] = new SolidColorBrush(System.Windows.Media.Colors.White);
            app.Resources["CardBgBrush"] = new SolidColorBrush(System.Windows.Media.Colors.White);
            app.Resources["SurfaceBrush"] = new SolidColorBrush(System.Windows.Media.Colors.WhiteSmoke);
        }
        else
        {
            var theme = (AppTheme)SelectedThemeIndex;
            ThemeManager.ApplyTheme(theme);
        }

        var s = AppSettings.Load();
        s.IsHighContrast = IsHighContrast;
        s.Save();
    }

    [RelayCommand]
    private void SaveSettings()
    {
        var s = AppSettings.Load();
        s.ShopName = BusinessName;
        s.Phone = BusinessPhone;
        s.Address = BusinessAddress;
        s.VatPercentage = VatPercentage;
        s.CommissionPercentage = CommissionPercentage;
        s.CurrencySymbol = CurrencySymbol;
        s.ReceiptFooter = ReceiptFooter;
        s.ReceiptHeader = ReceiptHeader;
        s.InvoicePrefix = InvoicePrefix;
        s.Save();
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
            System.IO.File.Copy(dbPath, backupPath, true);
    }

    [RelayCommand]
    private void RestoreDatabase()
    {
    }
}
