using System.IO;
using System.Windows;
using System.Windows.Threading;
using PosAccountingApp.Models;
using PosAccountingApp.Views;

namespace PosAccountingApp;

public partial class App : Application
{
    private void OnStartup(object sender, StartupEventArgs e)
    {
        // Global error handler - never crash, show dialog instead
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

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

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show(
            "خطا در برنامه:\n\n" + e.Exception.Message + "\n\n" + e.Exception.StackTrace,
            "خطای برنامه",
            MessageBoxButton.OK, MessageBoxImage.Error);
        e.Handled = true;
    }

    private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            MessageBox.Show("خطای بحرانی:\n\n" + ex.Message, "خطای بحرانی",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        MessageBox.Show("خطای پس‌زمینه:\n\n" + e.Exception.Message, "خطا",
            MessageBoxButton.OK, MessageBoxImage.Error);
        e.SetObserved();
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
    public string? CurrencySymbol { get; set; } = "ريال";
    public string? ReceiptFooter { get; set; } = "با تشکر از خرید شما";
    public string? ReceiptHeader { get; set; } = "فاکتور فروش";
    public string? InvoicePrefix { get; set; } = "INV";
    public bool UsePopupForAdd { get; set; } = true;
    public int AddModeIndex { get; set; } = 2;
    public int SelectedProfileIndex { get; set; } = 0;

    public static AppSettings Load()
    {
        if (File.Exists(SettingsPath))
        {
            var json = File.ReadAllText(SettingsPath);
            return System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        return new AppSettings();
    }

    public void Save()
    {
        var dir = Path.GetDirectoryName(SettingsPath);
        if (dir != null) Directory.CreateDirectory(dir);
        var json = System.Text.Json.JsonSerializer.Serialize(this, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(SettingsPath, json);
    }
}
