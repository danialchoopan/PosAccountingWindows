using System.Windows;
using System.Windows.Controls;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class SettingsView : UserControl
{
    private SettingsViewModel? _vm;

    public SettingsView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _vm = DataContext as SettingsViewModel;
        if (_vm == null) return;

        // Load current values into controls
        ShopNameBox.Text = _vm.BusinessName;
        PhoneBox.Text = _vm.BusinessPhone;
        AddressBox.Text = _vm.BusinessAddress;
        VatBox.Text = _vm.VatPercentage.ToString();
        CommissionBox.Text = _vm.CommissionPercentage.ToString();
        ReceiptHeaderBox.Text = _vm.ReceiptHeader;
        ReceiptFooterBox.Text = _vm.ReceiptFooter;
        ThemeCombo.SelectedIndex = _vm.SelectedThemeIndex;
        FontSizeCombo.SelectedIndex = GetFontSizeIndex(_vm.FontSize);
        HighContrastCheck.IsChecked = _vm.IsHighContrast;
        AddModeCombo.SelectedIndex = _vm.AddModeIndex;
        ProfileCombo.SelectedIndex = _vm.SelectedProfileIndex;
    }

    private static int GetFontSizeIndex(double size) => size switch
    {
        10 => 0, 12 => 1, 14 => 2, 16 => 3, 18 => 4, 20 => 5, _ => 2
    };

    private void Apply_Click(object sender, RoutedEventArgs e)
    {
        if (_vm == null) return;

        // Read values from controls into ViewModel
        _vm.BusinessName = ShopNameBox.Text ?? "";
        _vm.BusinessPhone = PhoneBox.Text ?? "";
        _vm.BusinessAddress = AddressBox.Text ?? "";
        _vm.SelectedThemeIndex = ThemeCombo.SelectedIndex;
        _vm.SelectedProfileIndex = ProfileCombo.SelectedIndex;
        _vm.IsHighContrast = HighContrastCheck.IsChecked == true;
        _vm.AddModeIndex = AddModeCombo.SelectedIndex;

        if (double.TryParse(FontSizeCombo.SelectedItem is ComboBoxItem fi ? fi.Content?.ToString() : null, out var fs))
            _vm.FontSize = fs;

        if (decimal.TryParse(VatBox.Text, out var vat))
            _vm.VatPercentage = vat;

        if (decimal.TryParse(CommissionBox.Text, out var comm))
            _vm.CommissionPercentage = comm;

        _vm.ReceiptHeader = ReceiptHeaderBox.Text ?? "";
        _vm.ReceiptFooter = ReceiptFooterBox.Text ?? "";

        _vm.SaveAndApply();

        StatusLabel.Text = _vm.StatusMessage;
        StatusLabel.Visibility = Visibility.Visible;
    }

    private void Backup_Click(object sender, RoutedEventArgs e)
    {
        _vm?.BackupDb();
        StatusLabel.Text = _vm?.StatusMessage ?? "";
        StatusLabel.Visibility = Visibility.Visible;
    }

    private void Seed_Click(object sender, RoutedEventArgs e)
    {
        _vm?.SeedSampleData();
        StatusLabel.Text = _vm?.StatusMessage ?? "";
        StatusLabel.Visibility = Visibility.Visible;
    }
}
