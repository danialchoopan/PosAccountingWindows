using System.IO;
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
    [ObservableProperty] private string _currencySymbol = "\u0631\u064A\u0627\u0644";
    [ObservableProperty] private string _receiptFooter = "\u0628\u0627 \u062A\u0634\u06A9\u0631 \u0627\u0632 \u062E\u0631\u06CC\u062F \u0634\u0645\u0627";
    [ObservableProperty] private string _receiptHeader = "\u0641\u0627\u06A9\u062A\u0648\u0631 \u0641\u0631\u0648\u0634";
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
            CurrencySymbol = s.CurrencySymbol ?? "\u0631\u064A\u0627\u0644";
            ReceiptFooter = s.ReceiptFooter ?? "\u0628\u0627 \u062A\u0634\u06A9\u0631 \u0627\u0632 \u062E\u0631\u06CC\u062F \u0634\u0645\u0627";
            ReceiptHeader = s.ReceiptHeader ?? "\u0641\u0627\u06A9\u062A\u0648\u0631 \u0641\u0631\u0648\u0634";
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

    public void SaveAndApply()
    {
        try
        {
            // 1. Apply theme
            var theme = (AppTheme)SelectedThemeIndex;
            ThemeManager.ApplyTheme(theme);

            // 2. Apply font size
            var app = Application.Current;
            if (app != null)
                app.Resources["AppFontSize"] = FontSize;

            // 3. Apply high contrast
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

            // 4. Save to file
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

            StatusMessage = "\u062A\u0636\u06CC\u0645\u06CC\u0645\u0627\u062A \u0630\u062E\u06CC\u0631\u0647 \u0648 \u0627\u0639\u0645\u0627\u0644 \u0634\u062F";
        }
        catch (Exception ex)
        {
            StatusMessage = "\u062E\u0637\u0627: " + ex.Message;
        }
    }

    public void SeedSampleData()
    {
        try
        {
            SeedData.Seed();
            StatusMessage = "\u062F\u0627\u062F\u0647 \u0646\u0645\u0648\u0646\u0647 \u0628\u0627 \u0645\u0648\u0641\u0642\u06CC\u062A \u0627\u06CC\u062C\u0627\u062F \u0634\u062F";
        }
        catch (Exception ex)
        {
            StatusMessage = "\u062E\u0637\u0627: " + ex.Message;
        }
    }

    public void BackupDb()
    {
        var dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PosAccountingApp", "pos_data.db");
        var backupPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            $"pos_backup_{DateTime.Now:yyyyMMdd_HHmmss}.db");
        if (File.Exists(dbPath))
        {
            File.Copy(dbPath, backupPath, true);
            StatusMessage = "\u067E\u0634\u062A\u06CC\u0628\u0627\u0646 \u0630\u062E\u06CC\u0631\u0647 \u0634\u062F";
        }
    }
}
