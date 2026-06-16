using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PosAccountingApp.Models;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class PosView : UserControl
{
    private bool _isUpdating;

    public PosView()
    {
        InitializeComponent();
        Loaded += (_, _) => BarcodeBox.Focus();
    }

    private PosViewModel? GetVm() => DataContext as PosViewModel;

    private void BarcodeBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_isUpdating) return;
        var vm = GetVm();
        if (vm == null) return;
        vm.BarcodeInput = BarcodeBox.Text?.Trim() ?? "";
    }

    private void BarcodeBox_KeyDown(object sender, KeyEventArgs e)
    {
        var vm = GetVm();
        if (vm == null) return;

        if (e.Key == Key.Down && SearchList.Items.Count > 0)
        {
            SearchList.SelectedIndex = 0;
            SearchList.Focus();
            e.Handled = true;
        }
        else if (e.Key == Key.Enter)
        {
            if (SearchList.SelectedItem is Product product)
            {
                vm.AddProductToCartCommand.Execute(product);
                ClearInput();
            }
            else if (!string.IsNullOrWhiteSpace(BarcodeBox.Text))
            {
                vm.BarcodeInput = BarcodeBox.Text.Trim();
                vm.AddByBarcodeCommand.Execute(null);
                ClearInput();
            }
        }
        else if (e.Key == Key.Escape)
        {
            vm.IsSearchOpen = false;
        }
    }

    private void SearchList_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && SearchList.SelectedItem is Product product)
        {
            GetVm()?.AddProductToCartCommand.Execute(product);
            ClearInput();
        }
        else if (e.Key == Key.Escape)
        {
            BarcodeBox.Focus();
        }
    }

    private void SearchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SearchList.SelectedItem is Product product)
        {
            GetVm()?.AddProductToCartCommand.Execute(product);
            ClearInput();
        }
    }

    private void ClearInput()
    {
        _isUpdating = true;
        BarcodeBox.Text = "";
        _isUpdating = false;
        BarcodeBox.Focus();
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
            ClearInput();
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
                $"آیا از حذف '{item.ProductTitle}' از سبد اطمینان دارید؟",
                "تایید حذف", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                GetVm()?.RemoveItemCommand.Execute(item);
        }
    }

    private void FullyPaid_Checked(object sender, RoutedEventArgs e) =>
        GetVm()?.SetFullyPaidCommand.Execute(true);

    private void FullyPaid_Unchecked(object sender, RoutedEventArgs e) =>
        GetVm()?.SetFullyPaidCommand.Execute(false);

    private void FinalizeBtn_Click(object sender, RoutedEventArgs e) =>
        GetVm()?.FinalizeSaleCommand.Execute(null);
}
