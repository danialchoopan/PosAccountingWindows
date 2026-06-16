using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PosAccountingApp.Models;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class SellsView : UserControl
{
    public SellsView()
    {
        InitializeComponent();
    }

    private void SalesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is DataGrid grid && grid.SelectedItem is Sale sale)
        {
            if (DataContext is SellsViewModel vm)
                vm.SelectSaleCommand.Execute(sale);
        }
    }
}
