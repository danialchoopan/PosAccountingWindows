using System.Data;
using System.Windows;
using System.Windows.Controls;
using PosAccountingApp.Controls;
using PosAccountingApp.Data;
using PosAccountingApp.Models;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class CustomersView : UserControl
{
    private CustomersViewModel? _vm;

    public CustomersView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is CustomersViewModel vm)
        {
            _vm = vm;
            PagedGrid.SetTitle("\u0644\u06CC\u0633\u062A \u0645\u0634\u062A\u0631\u06CC\u0627\u0646");
            PagedGrid.SetColumns(
                ("Name", "\u0646\u0627\u0645", 200),
                ("Phone", "\u062A\u0644\u0641\u0646", 140),
                ("Balance", "\u0645\u0648\u062C\u0648\u062F\u06CC \u062D\u0633\u0627\u0628", 120),
                ("CreditLimit", "\u0633\u0642\u0641 \u0627\u0639\u062A\u0628\u0627\u0631", 120),
                ("LoyaltyPoints", "\u0627\u0645\u062A\u06CC\u0627\u0632", 80)
            );
            PagedGrid.SetHeaders("\u0646\u0627\u0645", "\u062A\u0644\u0641\u0646", "\u0645\u0648\u062C\u0648\u062F\u06CC \u062D\u0633\u0627\u0628", "\u0633\u0642\u0641 \u0627\u0639\u062A\u0628\u0627\u0631", "\u0627\u0645\u062A\u06CC\u0627\u0632");
            PagedGrid.ItemDoubleClicked += OnItemDoubleClicked;
            RefreshGrid();
        }
    }

    public void RefreshGrid()
    {
        if (_vm == null) return;
        var table = new DataTable();
        table.Columns.Add("Name"); table.Columns.Add("Phone");
        table.Columns.Add("Balance"); table.Columns.Add("CreditLimit");
        table.Columns.Add("LoyaltyPoints"); table.Columns.Add("Id", typeof(int));
        foreach (var c in _vm.Customers)
            table.Rows.Add(c.Name, c.Phone, c.Balance, c.CreditLimit, c.LoyaltyPoints, c.Id);
        PagedGrid.LoadData(table);
    }

    private void OnItemDoubleClicked(object? item)
    {
        if (item is DataRow row)
        {
            var result = MessageBox.Show(
                $"\u0646\u0627\u0645: {row["Name"]}\n\u062A\u0644\u0641\u0646: {row["Phone"]}\n\u0645\u0648\u062C\u0648\u062F\u06CC: {row["Balance"]}\n\u0633\u0642\u0641 \u0627\u0639\u062A\u0628\u0627\u0631: {row["CreditLimit"]}\n\n\u0627\u0637\u0644\u0627\u0639\u0627\u062A \u0628\u0631\u0627\u06CC \u062D\u0630\u0641 \u062C\u0627\u0631\u06CC \u0627\u0633\u062A\u061F",
                "\u062D\u0630\u0641 \u0645\u0634\u062A\u0631\u06CC",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                _vm?.DeleteCustomerCommand.Execute(_vm.Customers.FirstOrDefault(c => c.Id == (int)row["Id"]));
        }
    }
}
