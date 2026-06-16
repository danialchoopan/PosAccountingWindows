using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PosAccountingApp.Models;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class PosView : UserControl
{
    private PosViewModel? _vm;
    private bool _isUpdating;

    public PosView()
    {
        InitializeComponent();
        DataContext = new PosViewModel();
        _vm = (PosViewModel)DataContext;
        BarcodeBox.Focus();
    }

    private void BarcodeBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_isUpdating) return;
        var code = BarcodeBox.Text?.Trim() ?? "";
        _vm.BarcodeInput = code;
    }

    private void BarcodeBox_KeyDown(object sender, KeyEventArgs e)
    {
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
                _vm.AddProductToCartCommand.Execute(product);
                _isUpdating = true;
                BarcodeBox.Text = "";
                _isUpdating = false;
                BarcodeBox.Focus();
            }
            else if (!string.IsNullOrWhiteSpace(BarcodeBox.Text))
            {
                _vm.BarcodeInput = BarcodeBox.Text.Trim();
                _vm.AddByBarcodeCommand.Execute(null);
                _isUpdating = true;
                BarcodeBox.Text = "";
                _isUpdating = false;
            }
        }
        else if (e.Key == Key.Escape)
        {
            _vm.IsSearchOpen = false;
        }
    }

    private void SearchList_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && SearchList.SelectedItem is Product product)
        {
            _vm.AddProductToCartCommand.Execute(product);
            _isUpdating = true;
            BarcodeBox.Text = "";
            _isUpdating = false;
            BarcodeBox.Focus();
        }
    }

    private void SearchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (SearchList.SelectedItem is Product product)
        {
            _vm.AddProductToCartCommand.Execute(product);
            _isUpdating = true;
            BarcodeBox.Text = "";
            _isUpdating = false;
            BarcodeBox.Focus();
        }
    }

    private void AddBtn_Click(object sender, RoutedEventArgs e)
    {
        var code = BarcodeBox.Text?.Trim();
        if (!string.IsNullOrEmpty(code))
        {
            _vm.BarcodeInput = code;
            _vm.AddByBarcodeCommand.Execute(null);
            _isUpdating = true;
            BarcodeBox.Text = "";
            _isUpdating = false;
        }
    }

    private void IncreaseBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is PosCartItem item)
            _vm.IncreaseQuantityCommand.Execute(item);
    }

    private void DecreaseBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is PosCartItem item)
            _vm.DecreaseQuantityCommand.Execute(item);
    }

    private void DeleteBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is PosCartItem item)
        {
            var result = MessageBox.Show(
                $"آیا از حذف '{item.ProductTitle}' از سبد اطمینان دارید؟",
                "تایید حذف", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                _vm.RemoveItemCommand.Execute(item);
        }
    }

    private void FullyPaid_Checked(object sender, RoutedEventArgs e)
    {
        _vm.SetFullyPaidCommand.Execute(true);
    }

    private void FullyPaid_Unchecked(object sender, RoutedEventArgs e)
    {
        _vm.SetFullyPaidCommand.Execute(false);
    }

    private void FinalizeBtn_Click(object sender, RoutedEventArgs e)
    {
        _vm.FinalizeSaleCommand.Execute(null);
    }
}
