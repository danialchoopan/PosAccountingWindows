using System.IO;
using System.Windows;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm;

    public MainWindow()
    {
        InitializeComponent();
        Data.DatabaseInitializer.Initialize();
        _vm = new MainViewModel();
        DataContext = _vm;
        LoadShopName();
    }

    private void LoadShopName()
    {
        try
        {
            var settingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PosAccountingApp", "settings.json");
            if (File.Exists(settingsPath))
            {
                var json = File.ReadAllText(settingsPath);
                var settings = System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json);
                if (settings != null)
                {
                    ShopNameLabel.Text = settings.ShopName;
                    Title = $"{settings.ShopName} - سیستم مدیریت فروش و حسابداری";
                }
            }
        }
        catch { }
    }
}
