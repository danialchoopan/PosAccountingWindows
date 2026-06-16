using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PosAccountingApp.Models;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class PosView : UserControl
{
    public PosView()
    {
        InitializeComponent();
        DataContext = new PosViewModel();
        BarcodeBox.Focus();
    }

    private PosViewModel? GetVm() => DataContext as PosViewModel;

    private void BarcodeBox_KeyDown(object sender, KeyEventArgs e)
    {
        var vm = GetVm();
        if (vm == null) return;

        if (e.Key == Key.Down && vm.SearchResults.Count > 0)
        {
            // Focus on search results
            e.Handled = true;
        }
        else if (e.Key == Key.Enter)
        {
            if (vm.SearchResults.Count > 0)
                vm.AddProductToCartCommand.Execute(vm.SearchResults[0]);
            else
            {
                var code = BarcodeBox.Text?.Trim();
                if (!string.IsNullOrEmpty(code))
                {
                    vm.BarcodeInput = code;
                    vm.AddByBarcodeCommand.Execute(null);
                }
            }
        }
        else if (e.Key == Key.Escape)
        {
            vm.IsSearchOpen = false;
        }
    }

    private void AddBtn_Click(object sender, RoutedEventArgs e)
    {
        var vm = GetVm();
        if (vm == null) return;
        var code = BarcodeBox.Text?.Trim();
        if (!string.IsNullOrEmpty(code))
        {
            vm.BarcodeInput = code;
            vm.AddByBarcodeCommand.Execute(null);
        }
    }

    private void IncreaseBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is PosCartItem item)
            GetVm()?.IncreaseQuantityCommand.Execute(item);
    }

    private void DecreaseBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is PosCartItem item)
            GetVm()?.DecreaseQuantityCommand.Execute(item);
    }

    private void DeleteBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is PosCartItem item)
        {
            var result = MessageBox.Show(
                $"آیا از حذف کالای '{item.ProductTitle}' از سبد اطمینان دارید؟",
                "تایید حذف", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                GetVm()?.RemoveItemCommand.Execute(item);
        }
    }

    private void SearchItem_Click(object sender, MouseButtonEventArgs e)
    {
        if (sender is Border border && border.Tag is Product product)
            GetVm()?.AddProductToCartCommand.Execute(product);
    }

    private void FullyPaid_Checked(object sender, RoutedEventArgs e)
    {
        GetVm()?.SetFullyPaidCommand.Execute(true);
    }

    private void FullyPaid_Unchecked(object sender, RoutedEventArgs e)
    {
        GetVm()?.SetFullyPaidCommand.Execute(false);
    }

    private void FinalizeBtn_Click(object sender, RoutedEventArgs e)
    {
        GetVm()?.FinalizeSaleCommand.Execute(null);
    }
}
