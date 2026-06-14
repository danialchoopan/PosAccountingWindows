using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PosAccountingApp.Data;

namespace PosAccountingApp.Controls;

public partial class PaginatedDataGrid : UserControl
{
    private DataTable _allData = new();
    private int _currentPage = 1;
    private int _pageSize = 10;
    private string _title = "گزارش";
    private bool _initialized;

    public event Action<object?>? ItemDoubleClicked;

    public PaginatedDataGrid()
    {
        InitializeComponent();
        _initialized = true;
    }

    public void SetTitle(string title) { _title = title; }

    public void SetColumns(params (string Binding, string Header, double Width)[] columns)
    {
        MainGrid.Columns.Clear();
        foreach (var (binding, header, width) in columns)
        {
            var col = new DataGridTextColumn
            {
                Header = header,
                Binding = new System.Windows.Data.Binding(binding),
                Width = new DataGridLength(width)
            };
            MainGrid.Columns.Add(col);
        }
    }

    public void LoadData(DataTable table)
    {
        _allData = table ?? new DataTable();
        _currentPage = 1;
        RefreshGrid();
    }

    public void SetHeaders(params string[] headers)
    {
        for (int i = 0; i < MainGrid.Columns.Count && i < headers.Length; i++)
            MainGrid.Columns[i].Header = headers[i];
    }

    private void RefreshGrid()
    {
        if (!_initialized || _allData == null || _allData.Rows.Count == 0)
        {
            MainGrid.ItemsSource = null;
            if (PageInfoText != null) PageInfoText.Text = "داده‌ای موجود نیست";
            if (PageNumbersText != null) PageNumbersText.Text = "";
            if (TotalText != null) TotalText.Text = "";
            return;
        }

        int totalRows = _allData.Rows.Count;
        bool showAll = _pageSize >= totalRows;
        int totalPages = showAll ? 1 : (int)Math.Ceiling((double)totalRows / _pageSize);

        if (_currentPage > totalPages) _currentPage = totalPages;
        if (_currentPage < 1) _currentPage = 1;

        if (!showAll)
        {
            int start = (_currentPage - 1) * _pageSize;
            int count = Math.Min(_pageSize, totalRows - start);

            var pageTable = _allData.Clone();
            for (int i = start; i < start + count && i < totalRows; i++)
                pageTable.ImportRow(_allData.Rows[i]);

            MainGrid.ItemsSource = pageTable.DefaultView;
        }
        else
        {
            MainGrid.ItemsSource = _allData.DefaultView;
        }

        int from = showAll ? 1 : (_currentPage - 1) * _pageSize + 1;
        int to = showAll ? totalRows : Math.Min(_currentPage * _pageSize, totalRows);

        PageInfoText.Text = $"نمایش {from} تا {to}";
        TotalText.Text = $"کل: {totalRows}";
        PageNumbersText.Text = totalPages > 1 ? $"صفحه {_currentPage} از {totalPages}" : "";
    }

    private void PageSize_Changed(object sender, SelectionChangedEventArgs e)
    {
        if (!_initialized) return;
        if (PageSizeCombo.SelectedItem is ComboBoxItem item)
        {
            _pageSize = item.Content.ToString() == "همه" ? int.MaxValue : int.Parse(item.Content.ToString()!);
            _currentPage = 1;
            RefreshGrid();
        }
    }

    private void FirstPage_Click(object sender, RoutedEventArgs e) { _currentPage = 1; RefreshGrid(); }
    private void PrevPage_Click(object sender, RoutedEventArgs e) { if (_currentPage > 1) { _currentPage--; RefreshGrid(); } }
    private void NextPage_Click(object sender, RoutedEventArgs e)
    {
        int totalPages = _allData.Rows.Count == 0 ? 1 : (int)Math.Ceiling((double)_allData.Rows.Count / _pageSize);
        if (_currentPage < totalPages) { _currentPage++; RefreshGrid(); }
    }
    private void LastPage_Click(object sender, RoutedEventArgs e)
    {
        _currentPage = _allData.Rows.Count == 0 ? 1 : (int)Math.Ceiling((double)_allData.Rows.Count / _pageSize);
        if (_currentPage < 1) _currentPage = 1;
        RefreshGrid();
    }

    private void MainGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (MainGrid.SelectedItem is DataRowView row)
            ItemDoubleClicked?.Invoke(row.Row);
    }

    private void ExportPdf_Click(object sender, RoutedEventArgs e)
    {
        if (_allData == null || _allData.Rows.Count == 0) return;
        ExportHelper.ExportToPdf(_allData, _title);
    }

    private void ExportExcel_Click(object sender, RoutedEventArgs e)
    {
        if (_allData == null || _allData.Rows.Count == 0) return;
        ExportHelper.ExportToExcel(_allData, _title);
    }

    private void Print_Click(object sender, RoutedEventArgs e)
    {
        if (_allData == null || _allData.Rows.Count == 0) return;
        ExportHelper.PrintTable(_allData, _title);
    }
}
