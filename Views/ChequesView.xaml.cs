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
            PagedGrid.SetTitle("لیست چک‌ها");
            PagedGrid.SetColumns(
                ("ChequeNumber", "شماره چک", 120), ("BankName", "بانک", 100),
                ("Amount", "مبلغ", 120), ("DueDate", "سررسید", 100),
                ("PayerName", "صاحب چک", 150), ("Status", "وضعیت", 80));
            PagedGrid.SetHeaders("شماره چک", "بانک", "مبلغ", "سررسید", "صاحب چک", "وضعیت");
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
            var win = new DetailEditWindow($"چک: {row["ChequeNumber"]}", row, "Cheque");
            win.Owner = Window.GetWindow(this);
            if (win.ShowDialog() == true) RefreshGrid();
        }
    }
}
