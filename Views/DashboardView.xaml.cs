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
        if (Window.GetWindow(this)?.DataContext is MainViewModel vm)
            return vm;
        return null;
    }

    private void NavigateToPOS(object sender, RoutedEventArgs e) =>
        GetMainViewModel()?.NavigateToCommand.Execute("POS");

    private void NavigateToProducts(object sender, RoutedEventArgs e) =>
        GetMainViewModel()?.NavigateToCommand.Execute("Products");

    private void NavigateToCustomers(object sender, RoutedEventArgs e) =>
        GetMainViewModel()?.NavigateToCommand.Execute("Customers");

    private void NavigateToCheques(object sender, RoutedEventArgs e) =>
        GetMainViewModel()?.NavigateToCommand.Execute("Cheques");

    private void NavigateToCategories(object sender, RoutedEventArgs e) =>
        GetMainViewModel()?.NavigateToCommand.Execute("Categories");

    private void NavigateToExpenses(object sender, RoutedEventArgs e) =>
        GetMainViewModel()?.NavigateToCommand.Execute("Expenses");

    private void NavigateToSuppliers(object sender, RoutedEventArgs e) =>
        GetMainViewModel()?.NavigateToCommand.Execute("Suppliers");

    private void NavigateToLoyalty(object sender, RoutedEventArgs e) =>
        GetMainViewModel()?.NavigateToCommand.Execute("Loyalty");

    private void NavigateToAccounting(object sender, RoutedEventArgs e) =>
        GetMainViewModel()?.NavigateToCommand.Execute("Accounting");

    private void NavigateToReports(object sender, RoutedEventArgs e) =>
        GetMainViewModel()?.NavigateToCommand.Execute("Reports");

    private void NavigateToUsers(object sender, RoutedEventArgs e) =>
        GetMainViewModel()?.NavigateToCommand.Execute("Users");

    private void NavigateToSettings(object sender, RoutedEventArgs e) =>
        GetMainViewModel()?.NavigateToCommand.Execute("Settings");
}
