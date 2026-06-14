using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PosAccountingApp.Views;

public partial class PosView : UserControl
{
    public PosView()
    {
        InitializeComponent();
    }

    private void BarcodeInput_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (DataContext is ViewModels.PosViewModel vm)
            {
                vm.AddByBarcodeCommand.Execute(null);
            }
        }
    }
}
