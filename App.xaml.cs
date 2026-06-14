using System.IO;
using System.Windows;
using PosAccountingApp.Models;
using PosAccountingApp.Views;

namespace PosAccountingApp;

public partial class App : Application
{
    private void OnStartup(object sender, StartupEventArgs e)
    {
        var settingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PosAccountingApp", "settings.json");

        if (File.Exists(settingsPath))
        {
            var json = File.ReadAllText(settingsPath);
            var settings = System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json);
            if (settings?.IsSetupComplete == true)
            {
                // Setup done - go to login
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                return;
            }
        }

        // First run
        var setupWindow = new SetupWindow();
        setupWindow.Show();
    }
}

// Global settings holder
public class AppSettings
{
    private static AppSettings? _instance;
    public static AppSettings? CurrentInstance => _instance;
    public static User? CurrentUser { get; set; }

    public string ShopName { get; set; } = "";
    public string BusinessType { get; set; } = "Supermarket";
    public string Phone { get; set; } = "";
    public string Address { get; set; } = "";
    public decimal VatPercentage { get; set; } = 10;
    public decimal CommissionPercentage { get; set; } = 2;
    public bool IsSetupComplete { get; set; }

    public static AppSettings Load()
    {
        var settingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PosAccountingApp", "settings.json");

        if (File.Exists(settingsPath))
        {
            var json = File.ReadAllText(settingsPath);
            _instance = System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        else
        {
            _instance = new AppSettings();
        }
        return _instance;
    }
}
