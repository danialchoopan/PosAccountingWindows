using System.Collections.Specialized;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using PosAccountingApp.Controls;
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
            PagedGrid.SetTitle("لیست کالاها");
            PagedGrid.SetColumns(
                ("Title", "عنوان کالا", 200),
                ("Barcode", "بارکد", 120),
                ("Category", "دسته", 100),
                ("Stock", "موجودی", 80),
                ("SalePrice", "قیمت فروش", 120),
                ("PurchasePrice", "قیمت خرید", 120)
            );
            PagedGrid.SetHeaders("عنوان کالا", "بارکد", "دسته", "موجودی", "قیمت فروش", "قیمت خرید");
            PagedGrid.ItemDoubleClicked += OnItemDoubleClicked;

            // Refresh when products change
            vm.Products.CollectionChanged += (_, _) => RefreshGrid();

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
            var win = new DetailWindow($"جزئیات کالا: {row["Title"]}", row);
            win.Owner = Window.GetWindow(this);
            win.ShowDialog();
        }
    }
}
