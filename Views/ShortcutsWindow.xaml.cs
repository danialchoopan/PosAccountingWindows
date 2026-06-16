using System.Data;
using System.IO;
using System.Windows;
using PosAccountingApp.Data;

namespace PosAccountingApp.Views;

public partial class ShortcutsWindow : Window
{
    public ShortcutsWindow()
    {
        InitializeComponent();
        LoadShortcuts();
    }

    private void LoadShortcuts()
    {
        var shortcuts = ShortcutsManager.LoadShortcuts();
        var table = new System.Data.DataTable();
        table.Columns.Add("Key", typeof(string));
        table.Columns.Add("Description", typeof(string));
        table.Columns.Add("Action", typeof(string));
        foreach (var s in shortcuts)
            table.Rows.Add(s.Key, s.Description, s.Action);
        ShortcutsGrid.ItemsSource = table.DefaultView;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (ShortcutsGrid.ItemsSource is DataView view)
        {
            var shortcuts = new System.Collections.Generic.List<AppShortcut>();
            foreach (System.Data.DataRow row in view.Table.Rows)
            {
                shortcuts.Add(new AppShortcut
                {
                    Key = row["Key"]?.ToString() ?? "",
                    Description = row["Description"]?.ToString() ?? "",
                    Action = row["Action"]?.ToString() ?? ""
                });
            }
            ShortcutsManager.SaveShortcuts(shortcuts);
            MessageBox.Show("ذخیره شد", "موفق", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void Reset_Click(object sender, RoutedEventArgs e)
    {
        File.Delete(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PosAccountingApp", "shortcuts.json"));
        LoadShortcuts();
        MessageBox.Show("بازگشت به پیش‌فرض", "موفق", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
