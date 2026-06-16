using System.IO;
using System.Windows;
using PosAccountingApp.Models;
using PosAccountingApp.Views;

namespace PosAccountingApp;

public partial class App : Application
{
    private static readonly string LogPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "PosAccountingApp", "errors.log");
    private static bool _isHandlingError;

    private void OnStartup(object sender, StartupEventArgs e)
    {
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            if (args.ExceptionObject is Exception ex) SafeLogAndShow(ex);
        };
        TaskScheduler.UnobservedTaskException += (_, args) =>
        {
            SafeLogAndShow(args.Exception);
            args.SetObserved();
        };
        DispatcherUnhandledException += (_, args) =>
        {
            SafeLogAndShow(args.Exception);
            args.Handled = true;
        };

        try
        {
            Data.ThemeManager.ApplyTheme(Data.AppTheme.OceanBlue);
            Data.DatabaseInitializer.Initialize();

            var settingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PosAccountingApp", "settings.json");

            if (!File.Exists(settingsPath))
            {
                var setupWindow = new SetupWindow();
                setupWindow.ShowDialog();
                if (!File.Exists(settingsPath)) { Shutdown(); return; }
            }

            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();

            if (AppSettings.CurrentUser != null)
            {
                var mainWindow = new MainWindow();
                mainWindow.ShowDialog();
            }

            Shutdown();
        }
        catch (Exception ex)
        {
            SafeLogAndShow(ex);
            Shutdown();
        }
    }

    private static void SafeLogAndShow(Exception ex)
    {
        if (_isHandlingError) return;
        _isHandlingError = true;

        // Always log to file first
        LogError(ex);

        // Then try to show dialog safely
        try
        {
            MessageBox.Show(
                $"خطا رخ داد:\n\n{ex.Message}\n\nجزئیات در فایل errors.log ذخیره شد.",
                "خطای برنامه",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch { }

        _isHandlingError = false;
    }

    public static void LogError(Exception ex)
    {
        try
        {
            var dir = Path.GetDirectoryName(LogPath);
            if (dir != null) Directory.CreateDirectory(dir);
            File.AppendAllText(LogPath,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex.Message}\n{ex.StackTrace}\n\n");
        }
        catch { }
    }

    private void OnExit(object sender, ExitEventArgs e) { }
}

public class AppSettings
{
    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "PosAccountingApp", "settings.json");

    public static User? CurrentUser { get; set; }

    public string ShopName { get; set; } = "";
    public string BusinessType { get; set; } = "Supermarket";
    public string Phone { get; set; } = "";
    public string Address { get; set; } = "";
    public decimal VatPercentage { get; set; } = 10;
    public decimal CommissionPercentage { get; set; } = 2;
    public bool IsSetupComplete { get; set; }
    public string SelectedTheme { get; set; } = "OceanBlue";
    public double FontSize { get; set; } = 14;
    public bool IsHighContrast { get; set; }
    public string? CurrencySymbol { get; set; } = "\u0631\u064A\u0627\u0644";
    public string? ReceiptFooter { get; set; } = "\u0628\u0627 \u062A\u0634\u06A9\u0631 \u0627\u0632 \u062E\u0631\u06CC\u062F \u0634\u0645\u0627";
    public string? ReceiptHeader { get; set; } = "\u0641\u0627\u06A9\u062A\u0648\u0631 \u0641\u0631\u0648\u0634";
    public string? InvoicePrefix { get; set; } = "INV";
    public bool UsePopupForAdd { get; set; } = true;
    public int AddModeIndex { get; set; } = 2;
    public int SelectedProfileIndex { get; set; } = 0;

    public static AppSettings Load()
    {
        if (File.Exists(SettingsPath))
        {
            try
            {
                var json = File.ReadAllText(SettingsPath);
                return System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            catch { return new AppSettings(); }
        }
        return new AppSettings();
    }

    public void Save()
    {
        try
        {
            var dir = Path.GetDirectoryName(SettingsPath);
            if (dir != null) Directory.CreateDirectory(dir);
            var json = System.Text.Json.JsonSerializer.Serialize(this, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsPath, json);
        }
        catch { }
    }
}
