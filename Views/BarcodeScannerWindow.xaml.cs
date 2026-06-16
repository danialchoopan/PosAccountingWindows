using System;
using System.Windows;
using System.Windows.Input;

namespace PosAccountingApp.Views;

public partial class BarcodeScannerWindow : Window
{
    public event Action<string>? BarcodeScanned;

    public BarcodeScannerWindow()
    {
        InitializeComponent();
        ManualInput.Focus();
    }

    private void ManualInput_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            ManualAdd_Click(sender, e);
    }

    private void ManualAdd_Click(object sender, RoutedEventArgs e)
    {
        var code = ManualInput.Text?.Trim();
        if (string.IsNullOrEmpty(code)) return;

        ScanResultText.Text = code;
        ScanHistory.Items.Add($"{DateTime.Now:HH:mm:ss}  -  {code}");
        BarcodeScanned?.Invoke(code);
        ManualInput.Text = string.Empty;
        ManualInput.Focus();
    }

    private void StartCamBtn_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(
            "برای استفاده از دوربین، کتابخانه OpenCvSharp را نصب کنید:\n\n" +
            "dotnet add package OpenCvSharp4\n" +
            "dotnet add package OpenCvSharp4.runtime.win\n\n" +
            "یا از اسکنر USB یا ورود دستی استفاده کنید.",
            "دوربین", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void StopCamBtn_Click(object sender, RoutedEventArgs e)
    {
        NoCameraText.Visibility = Visibility.Visible;
        CameraPreview.Source = null;
        StartCamBtn.IsEnabled = true;
        StopCamBtn.IsEnabled = false;
    }

    private void Close_Click(object sender, RoutedEventArgs e) => Close();
}
