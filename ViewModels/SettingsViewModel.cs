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

    public BusinessProfile[] Profiles { get; } = Enum.GetValues<BusinessProfile>();
    public RoundingMode[] RoundingModes { get; } = Enum.GetValues<RoundingMode>();

    [RelayCommand]
    private void SaveSettings()
    {
        // TODO: Persist to app settings file
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
        }
    }

    [RelayCommand]
    private void RestoreDatabase()
    {
        // TODO: Open file dialog and restore
    }
}
