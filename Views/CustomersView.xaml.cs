using System.Data;
using System.Windows;
using System.Windows.Controls;
using PosAccountingApp.Controls;
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
            PagedGrid.SetTitle("لیست مشتریان");
            PagedGrid.SetColumns(
                ("Name", "نام", 200),
                ("Phone", "تلفن", 140),
                ("Balance", "موجودی حساب", 120),
                ("CreditLimit", "سقف اعتبار", 120),
                ("LoyaltyPoints", "امتیاز", 80)
            );
            PagedGrid.SetHeaders("نام", "تلفن", "موجودی حساب", "سقف اعتبار", "امتیاز وفاداری");
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
            var win = new DetailWindow($"جزئیات مشتری: {row["Name"]}", row);
            win.Owner = Window.GetWindow(this);
            win.ShowDialog();
        }
    }
}
