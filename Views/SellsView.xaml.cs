using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PosAccountingApp.Data;
using PosAccountingApp.Models;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class SellsView : UserControl
{
    private SellsViewModel? _vm;

    public SellsView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _vm = DataContext as SellsViewModel;
    }

    private void SalesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is DataGrid grid && grid.SelectedItem is Sale sale)
            _vm?.SelectSaleCommand.Execute(sale);
    }

    private void ReturnBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is Sale sale)
        {
            if (sale.Status == SaleStatus.Returned)
            {
                ConfirmHelper.ShowWarning("\u0627\u06CC\u0646 \u0641\u0627\u06A9\u062A\u0648\u0631 \u0642\u0628\u0644\u0627 \u0645\u0631\u062C\u0648\u0639 \u0634\u062F\u0647 \u0627\u0633\u062A.");
                return;
            }

            var confirmed = ConfirmHelper.ConfirmReturn(sale.InvoiceNumber, sale.TotalAmount);
            if (confirmed)
                _vm?.ReturnSaleCommand.Execute(sale);
        }
    }
}
