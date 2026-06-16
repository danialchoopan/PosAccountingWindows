using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.Views;

public partial class InvoicePreviewWindow : Window
{
    private readonly Sale _sale;
    private bool _isStarred;

    public InvoicePreviewWindow(Sale sale)
    {
        _sale = sale;
        InitializeComponent();
        LoadInvoice();
    }

    private void LoadInvoice()
    {
        try
        {
            var s = AppSettings.Load();
            ShopNameLabel.Text = s.ShopName ?? "فروشگاه";
            InvoiceNumLabel.Text = $"شماره فاکتور: {_sale.InvoiceNumber}";
            DateLabel.Text = $"تاریخ: {_sale.CreatedAt:yyyy/MM/dd HH:mm}";
            SubtotalLabel.Text = $"{_sale.Subtotal:N0} ریال";
            TaxLabel.Text = $"{_sale.TaxAmount:N0} ریال";
            TotalLabel.Text = $"{_sale.TotalAmount:N0} ریال";
            StatusLabel.Text = _sale.Status == SaleStatus.Returned ? "مرجوع شده" : "پرداخت شده";
            FooterLabel.Text = s.ReceiptFooter ?? "با تشکر از خرید شما";

            // Load items
            using var db = DatabaseInitializer.CreateDbContext();
            var items = db.SaleItems.Where(i => i.SaleId == _sale.Id && i.IsActive).ToList();
            var table = new System.Data.DataTable();
            table.Columns.Add("ProductTitle", typeof(string));
            table.Columns.Add("Quantity", typeof(decimal));
            table.Columns.Add("UnitPrice", typeof(decimal));
            table.Columns.Add("Subtotal", typeof(decimal));
            foreach (var item in items)
                table.Rows.Add(item.ProductTitle, item.Quantity, item.UnitPrice, item.Subtotal);
            ItemsGrid.ItemsSource = table.DefaultView;
        }
        catch { }
    }

    private void Star_Click(object sender, RoutedEventArgs e)
    {
        _isStarred = !_isStarred;
        StarBtn.Content = _isStarred ? "ستاره‌دار شد" : "ستاره‌دار کردن";
    }

    private void Print_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var s = AppSettings.Load();
            var doc = new FlowDocument
            {
                PageWidth = 842,
                PageHeight = 595,
                PagePadding = new Thickness(40)
            };

            // Find a font that supports Persian
            var fontPath = System.IO.Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts),
                "tahoma.ttf");
            var fontFamily = System.IO.File.Exists(fontPath)
                ? new FontFamily(new Uri(fontPath), "Tahoma")
                : new FontFamily("Segoe UI");

            // Title
            doc.Blocks.Add(new Paragraph(new Run(s.ShopName ?? "فروشگاه") { FontFamily = fontFamily })
            {
                FontSize = 22, FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(0, 120, 212)),
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 5)
            });

            // Invoice info
            doc.Blocks.Add(new Paragraph(new Run($"{_sale.InvoiceNumber}") { FontFamily = fontFamily })
            { FontSize = 11, Foreground = Brushes.Gray, TextAlignment = TextAlignment.Center, Margin = new Thickness(0, 0, 0, 2) });

            doc.Blocks.Add(new Paragraph(new Run($"{_sale.CreatedAt:yyyy/MM/dd HH:mm}") { FontFamily = fontFamily })
            { FontSize = 11, Foreground = Brushes.Gray, TextAlignment = TextAlignment.Center, Margin = new Thickness(0, 0, 0, 12) });

            doc.Blocks.Add(new Paragraph(new Run("────────────────────────") { FontFamily = fontFamily })
            { Foreground = Brushes.LightGray, FontSize = 8, TextAlignment = TextAlignment.Center });

            // Items table
            using var db = DatabaseInitializer.CreateDbContext();
            var items = db.SaleItems.Where(i => i.SaleId == _sale.Id && i.IsActive).ToList();

            var table = new Table { BorderThickness = new Thickness(1), BorderBrush = Brushes.LightGray };
            table.Columns.Add(new TableColumn { Width = new GridLength(2, GridUnitType.Star) });
            table.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
            table.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });
            table.Columns.Add(new TableColumn { Width = new GridLength(1, GridUnitType.Star) });

            var headerRow = new TableRow { Background = new SolidColorBrush(Color.FromRgb(0, 120, 212)) };
            foreach (var h in new[] { "کالا", "تعداد", "قیمت", "جمع" })
                headerRow.Cells.Add(new TableCell(new Paragraph(new Run(h) { FontFamily = fontFamily })
                { Foreground = Brushes.White, FontWeight = FontWeights.Bold, Padding = new Thickness(6) }));
            var hg = new TableRowGroup(); hg.Rows.Add(headerRow); table.RowGroups.Add(hg);

            var dg = new TableRowGroup();
            foreach (var item in items)
            {
                var row = new TableRow();
                foreach (var v in new[] { item.ProductTitle, item.Quantity.ToString(), $"{item.UnitPrice:N0}", $"{item.Subtotal:N0}" })
                    row.Cells.Add(new TableCell(new Paragraph(new Run(v) { FontFamily = fontFamily }))
                    { Padding = new Thickness(6) });
                dg.Rows.Add(row);
            }
            table.RowGroups.Add(dg);
            doc.Blocks.Add(table);

            doc.Blocks.Add(new Paragraph(new Run("────────────────────────") { FontFamily = fontFamily })
            { Foreground = Brushes.LightGray, FontSize = 8, TextAlignment = TextAlignment.Center, Margin = new Thickness(0, 8, 0, 8) });

            // Totals
            doc.Blocks.Add(new Paragraph(new Run($"جمع کل: {_sale.Subtotal:N0} ریال") { FontFamily = fontFamily })
            { FontSize = 12, TextAlignment = TextAlignment.Left, Margin = new Thickness(0, 0, 0, 3) });
            doc.Blocks.Add(new Paragraph(new Run($"مالیات: {_sale.TaxAmount:N0} ریال") { FontFamily = fontFamily })
            { FontSize = 11, Foreground = Brushes.Gray, TextAlignment = TextAlignment.Left, Margin = new Thickness(0, 0, 0, 5) });
            doc.Blocks.Add(new Paragraph(new Run($"مبلغ قابل پرداخت: {_sale.TotalAmount:N0} ریال") { FontFamily = fontFamily })
            { FontSize = 15, FontWeight = FontWeights.Bold, TextAlignment = TextAlignment.Left, Margin = new Thickness(0, 5, 0, 10) });

            doc.Blocks.Add(new Paragraph(new Run("────────────────────────") { FontFamily = fontFamily })
            { Foreground = Brushes.LightGray, FontSize = 8, TextAlignment = TextAlignment.Center });
            doc.Blocks.Add(new Paragraph(new Run(s.ReceiptFooter ?? "با تشکر از خرید شما") { FontFamily = fontFamily })
            { FontSize = 11, Foreground = Brushes.Gray, TextAlignment = TextAlignment.Center });

            // Print
            var pd = new PrintDialog();
            if (pd.ShowDialog() == true)
                pd.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, s.ShopName ?? "فاکتور");
        }
        catch (Exception ex)
        {
            App.LogError(ex);
            MessageBox.Show("خطا در چاپ: " + ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Close_Click(object sender, RoutedEventArgs e) { Close(); }
}
