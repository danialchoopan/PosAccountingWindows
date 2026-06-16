using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class LoyaltyView : UserControl
{
    public LoyaltyView()
    {
        InitializeComponent();
    }

    private void TierGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is DataGrid grid && grid.SelectedItem is CustomerTierInfo info)
        {
            if (DataContext is LoyaltyViewModel vm)
                vm.SelectCustomerCommand.Execute(info);
        }
    }
}
