using System.Windows;
using System.Windows.Controls;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    private void AddMode_Changed(object sender, SelectionChangedEventArgs e)
    {
        if (AddModeCombo.SelectedItem is ComboBoxItem item && DataContext is SettingsViewModel vm)
        {
            vm.UsePopupForAdd = item.Tag?.ToString() == "True";
        }
    }
}
