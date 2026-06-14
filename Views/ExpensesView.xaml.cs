using System.Collections.Specialized;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using PosAccountingApp.Controls;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class ExpensesView : UserControl
{
    private ExpensesViewModel? _vm;

    public ExpensesView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ExpensesViewModel vm)
        {
            _vm = vm;
            PagedGrid.SetTitle("لیست هزینه‌ها");
            PagedGrid.SetColumns(
                ("Category", "دسته", 120),
                ("Description", "توضیحات", 250),
                ("Amount", "مبلغ", 120),
                ("Date", "تاریخ", 100)
            );
            PagedGrid.SetHeaders("دسته", "توضیحات", "مبلغ", "تاریخ");
            PagedGrid.ItemDoubleClicked += OnItemDoubleClicked;
            vm.Expenses.CollectionChanged += (_, _) => RefreshGrid();
            RefreshGrid();
        }
    }

    public void RefreshGrid()
    {
        if (_vm == null) return;
        var table = new DataTable();
        table.Columns.Add("Category"); table.Columns.Add("Description");
        table.Columns.Add("Amount"); table.Columns.Add("Date");
        table.Columns.Add("Id", typeof(int));
        foreach (var e in _vm.Expenses)
            table.Rows.Add(e.Category, e.Description, e.Amount, e.Date.ToString("yyyy/MM/dd"), e.Id);
        PagedGrid.LoadData(table);
    }

    private void OnItemDoubleClicked(object? item)
    {
        if (item is DataRow row)
        {
            var win = new DetailWindow($"جزئیات هزینه: {row["Description"]}", row);
            win.Owner = Window.GetWindow(this);
            win.ShowDialog();
        }
    }
}
