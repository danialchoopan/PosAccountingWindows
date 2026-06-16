using System.Data;
using System.Windows;
using System.Windows.Controls;
using PosAccountingApp.Controls;
using PosAccountingApp.Data;
using PosAccountingApp.Models;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class ChequesView : UserControl
{
    private ChequesViewModel? _vm;

    public ChequesView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ChequesViewModel vm)
        {
            _vm = vm;
            PagedGrid.SetTitle("\u0644\u06CC\u0633\u062A \u0686\u06A9\u200C\u0647\u0627");
            PagedGrid.SetColumns(
                ("ChequeNumber", "\u0634\u0645\u0627\u0631\u0647 \u0686\u06A9", 120),
                ("BankName", "\u0628\u0627\u0646\u06A9", 100),
                ("Amount", "\u0645\u0628\u0644\u063A", 120),
                ("DueDate", "\u0633\u0631\u0631\u0633\u06CC\u062F", 100),
                ("PayerName", "\u0635\u0627\u062D\u0628 \u0686\u06A9", 150),
                ("Status", "\u0648\u0636\u0639\u06CC\u062A", 80)
            );
            PagedGrid.SetHeaders("\u0634\u0645\u0627\u0631\u0647", "\u0628\u0627\u0646\u06A9", "\u0645\u0628\u0644\u063A", "\u0633\u0631\u0631\u0633\u06CC\u062F", "\u0635\u0627\u062D\u0628", "\u0648\u0636\u0639\u06CC\u062A");
            PagedGrid.ItemDoubleClicked += OnItemDoubleClicked;
            RefreshGrid();
        }
    }

    public void RefreshGrid()
    {
        if (_vm == null) return;
        var table = new DataTable();
        table.Columns.Add("ChequeNumber"); table.Columns.Add("BankName");
        table.Columns.Add("Amount"); table.Columns.Add("DueDate");
        table.Columns.Add("PayerName"); table.Columns.Add("Status");
        table.Columns.Add("Id", typeof(int));
        foreach (var c in _vm.Cheques)
            table.Rows.Add(c.ChequeNumber, c.BankName, c.Amount, c.DueDate.ToString("yyyy/MM/dd"), c.PayerName, c.Status, c.Id);
        PagedGrid.LoadData(table);
    }

    private void OnItemDoubleClicked(object? item)
    {
        if (item is DataRow row)
        {
            var result = MessageBox.Show(
                $"\u0634\u0645\u0627\u0631\u0647: {row["ChequeNumber"]}\n\u0628\u0627\u0646\u06A9: {row["BankName"]}\n\u0645\u0628\u0644\u063A: {row["Amount"]}\n\u0635\u0627\u062D\u0628: {row["PayerName"]}\n\n\u0627\u0637\u0644\u0627\u0639\u0627\u062A \u0628\u0631\u0627\u06CC \u062D\u0630\u0641 \u062C\u0627\u0631\u06CC \u0627\u0633\u062A\u061F",
                "\u062D\u0630\u0641 \u0686\u06A9",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                ConfirmHelper.ShowSuccess("\u0686\u06A9 \u062D\u0630\u0641 \u0634\u062F");
        }
    }
}
