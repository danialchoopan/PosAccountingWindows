using System.Windows;
using System.Windows.Input;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class GlobalSearchWindow : Window
{
    private readonly GlobalSearchViewModel _vm;

    public GlobalSearchWindow()
    {
        InitializeComponent();
        _vm = new GlobalSearchViewModel();
        DataContext = _vm;
        SearchBox.Focus();

        // ESC to close
        PreviewKeyDown += (s, e) =>
        {
            if (e.Key == Key.Escape)
                Close();
        };
    }

    private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        _vm.SearchText = SearchBox.Text;
    }
}
