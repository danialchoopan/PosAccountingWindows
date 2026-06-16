using System.Data;
using System.Windows;
using System.Windows.Controls;
using PosAccountingApp.Controls;
using PosAccountingApp.Data;
using PosAccountingApp.Models;
using PosAccountingApp.ViewModels;

namespace PosAccountingApp.Views;

public partial class ProductsView : UserControl
{
    private ProductsViewModel? _vm;

    public ProductsView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is ProductsViewModel vm)
        {
            _vm = vm;
            PagedGrid.SetTitle("\u0644\u06CC\u0633\u062A \u06A9\u0627\u0644\u0627\u0647\u0627");
            PagedGrid.SetColumns(
                ("Title", "\u0639\u0646\u0648\u0627\u0646", 200),
                ("Barcode", "\u0628\u0627\u0631\u06A9\u062F", 120),
                ("Category", "\u062F\u0633\u062A\u0647", 100),
                ("Stock", "\u0645\u0648\u062C\u0648\u062F\u06CC", 80),
                ("SalePrice", "\u0642\u06CC\u0645\u062A \u0641\u0631\u0648\u0634", 120),
                ("PurchasePrice", "\u0642\u06CC\u0645\u062A \u062E\u0631\u06CC\u062F", 120)
            );
            PagedGrid.SetHeaders("\u0639\u0646\u0648\u0627\u0646", "\u0628\u0627\u0631\u06A9\u062F", "\u062F\u0633\u062A\u0647", "\u0645\u0648\u062C\u0648\u062F\u06CC", "\u0642\u06CC\u0645\u062A \u0641\u0631\u0648\u0634", "\u0642\u06CC\u0645\u062A \u062E\u0631\u06CC\u062F");
            PagedGrid.ItemDoubleClicked += OnItemDoubleClicked;
            RefreshGrid();
        }
    }

    public void RefreshGrid()
    {
        if (_vm == null) return;
        var table = new DataTable();
        table.Columns.Add("Title", typeof(string));
        table.Columns.Add("Barcode", typeof(string));
        table.Columns.Add("Category", typeof(string));
        table.Columns.Add("Stock", typeof(string));
        table.Columns.Add("SalePrice", typeof(string));
        table.Columns.Add("PurchasePrice", typeof(string));
        table.Columns.Add("Id", typeof(int));
        foreach (var p in _vm.Products)
            table.Rows.Add(p.Title, p.Barcode ?? "-", p.Category ?? "-", p.Stock, p.SalePrice, p.PurchasePrice, p.Id);
        PagedGrid.LoadData(table);
    }

    private void OnItemDoubleClicked(object? item)
    {
        if (item is DataRow row)
        {
            var result = MessageBox.Show(
                $"\u06A9\u0627\u0644\u0627: {row["Title"]}\n\u0628\u0627\u0631\u06A9\u062F: {row["Barcode"]}\n\u0645\u0648\u062C\u0648\u062F\u06CC: {row["Stock"]}\n\u0642\u06CC\u0645\u062A \u0641\u0631\u0648\u0634: {row["SalePrice"]}\n\n\u0627\u0637\u0644\u0627\u0639\u0627\u062A \u0628\u0631\u0627\u06CC \u062D\u0630\u0641 \u062C\u0627\u0631\u06CC \u0627\u0633\u062A\u061F",
                "\u062D\u0630\u0641 \u06A9\u0627\u0644\u0627",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                _vm?.DeleteProductCommand.Execute(_vm.Products.FirstOrDefault(p => p.Id == (int)row["Id"]));
        }
    }
}
