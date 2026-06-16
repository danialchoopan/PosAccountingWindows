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
        BarcodeInput.Focus();
    }

    private void BarcodeInput_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            AddCode();
        }
    }

    private void AddBtn_Click(object sender, RoutedEventArgs e)
    {
        AddCode();
    }

    private void AddCode()
    {
        var code = BarcodeInput.Text?.Trim();
        if (string.IsNullOrEmpty(code)) return;

        ScanHistory.Items.Add($"{DateTime.Now:HH:mm:ss} - {code}");
        BarcodeScanned?.Invoke(code);
        BarcodeInput.Text = string.Empty;
        BarcodeInput.Focus();
    }

    private void Close_Click(object sender, RoutedEventArgs e) { Close(); }
}
