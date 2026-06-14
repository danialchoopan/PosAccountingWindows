using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty] private BusinessProfile _selectedProfile = BusinessProfile.Supermarket;
    [ObservableProperty] private string _businessName = "فروشگاه من";
    [ObservableProperty] private string _businessPhone = string.Empty;
    [ObservableProperty] private string _businessAddress = string.Empty;
    [ObservableProperty] private decimal _vatPercentage = 10;
    [ObservableProperty] private decimal _commissionPercentage = 2;
    [ObservableProperty] private RoundingMode _roundingMode = RoundingMode.Off;
    [ObservableProperty] private bool _isDarkTheme;

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
        IsDarkTheme = s.IsDarkTheme;
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        IsDarkTheme = !IsDarkTheme;
        ThemeManager.ApplyTheme(IsDarkTheme);
        var s = AppSettings.Load();
        s.IsDarkTheme = IsDarkTheme;
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
