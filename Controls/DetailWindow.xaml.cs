using System.Data;
using System.Windows;

namespace PosAccountingApp.Controls;

public partial class DetailWindow : Window
{
    public DetailWindow(string title, DataRow row)
    {
        InitializeComponent();
        TitleText.Text = title;
        Title = title;

        foreach (DataColumn col in row.Table.Columns)
        {
            var border = new System.Windows.Controls.Border
            {
                Padding = new Thickness(12, 10, 12, 10),
                Margin = new Thickness(0, 0, 0, 4),
                CornerRadius = new CornerRadius(6),
                Background = (System.Windows.Media.Brush)FindResource("CardBgBrush"),
                BorderBrush = (System.Windows.Media.Brush)FindResource("BorderBrush"),
                BorderThickness = new Thickness(1)
            };

            var grid = new System.Windows.Controls.Grid();
            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition { Width = new GridLength(120) });

            var valueText = new System.Windows.Controls.TextBlock
            {
                Text = row[col]?.ToString() ?? "-",
                FontSize = 14,
                FontWeight = FontWeights.Normal,
                VerticalAlignment = VerticalAlignment.Center
            };
            System.Windows.Controls.Grid.SetColumn(valueText, 0);

            var headerText = new System.Windows.Controls.TextBlock
            {
                Text = col.ColumnName,
                FontSize = 12,
                Foreground = (System.Windows.Media.Brush)FindResource("TextSecondaryBrush"),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            System.Windows.Controls.Grid.SetColumn(headerText, 1);

            grid.Children.Add(valueText);
            grid.Children.Add(headerText);
            border.Child = grid;
            DetailsPanel.Children.Add(border);
        }
    }

    private void Close_Click(object sender, RoutedEventArgs e) => Close();
}
