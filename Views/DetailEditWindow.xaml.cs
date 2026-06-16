using System.Data;
using System.Windows;
using System.Windows.Controls;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.Views;

public partial class DetailEditWindow : Window
{
    private readonly DataRow _row;
    private readonly string _tableName;
    private readonly int _entityId;

    public DetailEditWindow(string title, DataRow row, string tableName)
    {
        _row = row;
        _tableName = tableName;
        _entityId = row.Table?.Columns.Contains("Id") == true ? Convert.ToInt32(row["Id"]) : 0;

        InitializeComponent();
        TitleText.Text = title;
        Title = title;

        foreach (DataColumn col in row.Table.Columns)
        {
            if (col.ColumnName == "Id") continue;

            var sp = new StackPanel { Margin = new Thickness(4) };

            var header = new TextBlock
            {
                Text = col.ColumnName,
                FontSize = 12,
                Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryBrush"),
                Margin = new Thickness(0, 0, 0, 4)
            };

            var border = new Border
            {
                Padding = new Thickness(12, 8, 12, 8),
                CornerRadius = new CornerRadius(6),
                Background = (System.Windows.Media.Brush)FindResource("InputBgBrush"),
                BorderBrush = (System.Windows.Media.Brush)FindResource("InputBorderBrush"),
                BorderThickness = new Thickness(1)
            };

            var value = row[col]?.ToString() ?? "";

            var textBox = new TextBox
            {
                Text = value,
                FontSize = 14,
                Padding = new Thickness(8, 6, 8, 6),
                Background = System.Windows.Media.Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Tag = col.ColumnName
            };

            border.Child = textBox;
            sp.Children.Add(header);
            sp.Children.Add(border);
            FieldsPanel.Children.Add(sp);
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            var entry = db.Set<object>().Find(_entityId);
            if (entry == null)
            {
                // Try different entity types
                var entityType = _tableName;
                if (entityType == "Product")
                {
                    var p = db.Products.Find(_entityId);
                    if (p != null) UpdateEntity(p);
                }
                else if (entityType == "Customer")
                {
                    var c = db.Customers.Find(_entityId);
                    if (c != null) UpdateEntity(c);
                }
                else if (entityType == "Cheque")
                {
                    var ch = db.Cheques.Find(_entityId);
                    if (ch != null) UpdateEntity(ch);
                }
                else if (entityType == "Expense")
                {
                    var ex = db.Expenses.Find(_entityId);
                    if (ex != null) UpdateEntity(ex);
                }
                else if (entityType == "Supplier")
                {
                    var s = db.Suppliers.Find(_entityId);
                    if (s != null) UpdateEntity(s);
                }
                else if (entityType == "Sale")
                {
                    // Sales are read-only
                    MessageBox.Show("فاکتورها قابل ویرایش نیستند", "توجه", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }

            db.SaveChanges();
            MessageBox.Show("ذخیره شد", "موفق", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }
        catch (Exception ex)
        {
            App.LogError(ex);
            MessageBox.Show("خطا: " + ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void UpdateEntity(BaseEntity entity)
    {
        foreach (var child in FieldsPanel.Children)
        {
            if (child is StackPanel sp)
            {
                foreach (var c in sp.Children)
                {
                    if (c is Border border && border.Child is TextBox textBox && textBox.Tag is string fieldName)
                    {
                        var prop = entity.GetType().GetProperty(fieldName);
                        if (prop != null && prop.CanWrite)
                        {
                            var value = textBox.Text;
                            if (prop.PropertyType == typeof(decimal) && decimal.TryParse(value, out var d))
                                prop.SetValue(entity, d);
                            else if (prop.PropertyType == typeof(int) && int.TryParse(value, out var i))
                                prop.SetValue(entity, i);
                            else if (prop.PropertyType == typeof(bool))
                                prop.SetValue(entity, value.ToLower() == "true" || value == "1");
                            else
                                prop.SetValue(entity, value);
                        }
                    }
                }
            }
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("آیا از حذف این مورد اطمینان دارید؟", "تایید حذف",
            MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            using var db = DatabaseInitializer.CreateDbContext();
            var entityType = _tableName;

            if (entityType == "Product")
            {
                var p = db.Products.Find(_entityId);
                if (p != null) { p.IsActive = false; db.SaveChanges(); }
            }
            else if (entityType == "Customer")
            {
                var c = db.Customers.Find(_entityId);
                if (c != null) { c.IsActive = false; db.SaveChanges(); }
            }
            else if (entityType == "Cheque")
            {
                var ch = db.Cheques.Find(_entityId);
                if (ch != null) { ch.IsActive = false; db.SaveChanges(); }
            }
            else if (entityType == "Expense")
            {
                var ex = db.Expenses.Find(_entityId);
                if (ex != null) { ex.IsActive = false; db.SaveChanges(); }
            }
            else if (entityType == "Supplier")
            {
                var s = db.Suppliers.Find(_entityId);
                if (s != null) { s.IsActive = false; db.SaveChanges(); }
            }

            MessageBox.Show("حذف شد", "موفق", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }
        catch (Exception ex)
        {
            App.LogError(ex);
            MessageBox.Show("خطا: " + ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Close_Click(object sender, RoutedEventArgs e) { Close(); }
}
