using System.Data;
using System.Windows;
using System.Windows.Controls;
using PosAccountingApp.Controls;
using PosAccountingApp.Data;
using PosAccountingApp.Models;
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
            PagedGrid.SetTitle("\u0644\u06CC\u0633\u062A \u0647\u0632\u06CC\u0646\u0647\u200C\u0647\u0627");
            PagedGrid.SetColumns(
                ("Category", "\u062F\u0633\u062A\u0647", 120),
                ("Description", "\u062A\u0648\u0636\u06CC\u062D\u0627\u062A", 250),
                ("Amount", "\u0645\u0628\u0644\u063A", 120),
                ("Date", "\u062A\u0627\u0631\u06CC\u062E", 100)
            );
            PagedGrid.SetHeaders("\u062F\u0633\u062A\u0647", "\u062A\u0648\u0636\u06CC\u062D", "\u0645\u0628\u0644\u063A", "\u062A\u0627\u0631\u06CC\u062E");
            PagedGrid.ItemDoubleClicked += OnItemDoubleClicked;
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
            var result = MessageBox.Show(
                $"\u062A\u0648\u0636\u06CC\u062D: {row["Description"]}\n\u0645\u0628\u0644\u063A: {row["Amount"]}\n\u062A\u0627\u0631\u06CC\u062E: {row["Date"]}\n\n\u0627\u0637\u0644\u0627\u0639\u0627\u062A \u0628\u0631\u0627\u06CC \u062D\u0630\u0641 \u062C\u0627\u0631\u06CC \u0627\u0633\u062A\u061F",
                "\u062D\u0630\u0641 \u0647\u0632\u06CC\u0646\u0647",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                ConfirmHelper.ShowSuccess("\u0647\u0632\u06CC\u0646\u0647 \u062D\u0630\u0641 \u0634\u062F");
        }
    }
}
