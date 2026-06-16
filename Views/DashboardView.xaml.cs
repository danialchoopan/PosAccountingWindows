using System.Windows;
using System.Windows.Controls;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        InitializeComponent();
    }

    private MainViewModel? GetMainViewModel()
    {
        try { return Window.GetWindow(this)?.DataContext as MainViewModel; }
        catch { return null; }
    }

    private void NavigateToPOS(object sender, RoutedEventArgs e) =>
        Safe(() => GetMainViewModel()?.NavigateToCommand.Execute("POS"));

    private void NavigateToProducts(object sender, RoutedEventArgs e) =>
        Safe(() => GetMainViewModel()?.NavigateToCommand.Execute("Products"));

    private void NavigateToCustomers(object sender, RoutedEventArgs e) =>
        Safe(() => GetMainViewModel()?.NavigateToCommand.Execute("Customers"));

    private void NavigateToCheques(object sender, RoutedEventArgs e) =>
        Safe(() => GetMainViewModel()?.NavigateToCommand.Execute("Cheques"));

    private void NavigateToCategories(object sender, RoutedEventArgs e) =>
        Safe(() => GetMainViewModel()?.NavigateToCommand.Execute("Categories"));

    private void NavigateToExpenses(object sender, RoutedEventArgs e) =>
        Safe(() => GetMainViewModel()?.NavigateToCommand.Execute("Expenses"));

    private void NavigateToSuppliers(object sender, RoutedEventArgs e) =>
        Safe(() => GetMainViewModel()?.NavigateToCommand.Execute("Suppliers"));

    private void NavigateToAccounting(object sender, RoutedEventArgs e) =>
        Safe(() => GetMainViewModel()?.NavigateToCommand.Execute("Accounting"));

    private void NavigateToReports(object sender, RoutedEventArgs e) =>
        Safe(() => GetMainViewModel()?.NavigateToCommand.Execute("Reports"));

    private void NavigateToUsers(object sender, RoutedEventArgs e) =>
        Safe(() => GetMainViewModel()?.NavigateToCommand.Execute("Users"));

    private void NavigateToSettings(object sender, RoutedEventArgs e) =>
        Safe(() => GetMainViewModel()?.NavigateToCommand.Execute("Settings"));

    private static void Safe(Action action)
    {
        try { action(); }
        catch (Exception ex)
        {
            MessageBox.Show("خطا: " + ex.Message, "خطای برنامه", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
