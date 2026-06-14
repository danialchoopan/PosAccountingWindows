using System.IO;
using System.Windows;
using PosAccountingApp.Models;
using PosAccountingApp.Views;

namespace PosAccountingApp;

public partial class App : Application
{
    private void OnStartup(object sender, StartupEventArgs e)
    {
        Data.DatabaseInitializer.Initialize();

        var settingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PosAccountingApp", "settings.json");

        // First run: show setup dialog
        if (!File.Exists(settingsPath))
        {
            var setupWindow = new SetupWindow();
            setupWindow.ShowDialog();

            // If setup was cancelled (settings still doesn't exist), exit
            if (!File.Exists(settingsPath))
            {
                Shutdown();
                return;
            }
        }

        // Show login window
        var loginWindow = new LoginWindow();
        loginWindow.ShowDialog();

        // If login was successful (CurrentUser is set), open main window
        if (AppSettings.CurrentUser != null)
        {
            var mainWindow = new MainWindow();
            mainWindow.ShowDialog();
        }

        // Exit after main window closes
        Shutdown();
    }

    private void OnExit(object sender, ExitEventArgs e)
    {
    }
}

public class AppSettings
{
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
            return System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        return new AppSettings();
    }
}
