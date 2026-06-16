using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PosAccountingApp.Models;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class SupplierView : UserControl
{
    public SupplierView()
    {
        InitializeComponent();
    }

    private void SupplierGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is DataGrid grid && grid.SelectedItem is Supplier supplier)
        {
            if (DataContext is SupplierViewModel vm)
                vm.SelectSupplierCommand.Execute(supplier);
        }
    }

    private void PayBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is Supplier supplier)
        {
            if (DataContext is SupplierViewModel vm)
            {
                vm.SelectSupplierCommand.Execute(supplier);
                vm.OpenPaymentPanelCommand.Execute(null);
            }
        }
    }

    private void DeleteBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is Supplier supplier)
        {
            if (DataContext is SupplierViewModel vm)
                vm.DeleteSupplierCommand.Execute(supplier);
        }
    }
}
