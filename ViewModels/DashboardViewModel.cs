using CommunityToolkit.Mvvm.ComponentModel;

namespace PosAccountingApp.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty] private string _totalSalesToday = "---";
    [ObservableProperty] private string _totalRevenueToday = "---";
    [ObservableProperty] private string _totalCustomers = "---";
    [ObservableProperty] private string _totalProducts = "---";
    [ObservableProperty] private string _pendingCheques = "---";
    [ObservableProperty] private string _lowStockItems = "---";

    public void LoadData()
    {
        try
        {
            using var db = Data.DatabaseInitializer.CreateDbContext();
            var today = DateTime.Today;
            var salesToday = db.Sales.Where(s => s.CreatedAt >= today).ToList();

            TotalSalesToday = ToPersian(salesToday.Count) + " فاکتور";
            TotalRevenueToday = ToPersian((long)salesToday.Sum(s => s.TotalAmount)) + " ریال";
            TotalCustomers = ToPersian(db.Customers.Count());
            TotalProducts = ToPersian(db.Products.Count());
            PendingCheques = ToPersian(db.Cheques.Count(c => c.Status == Models.ChequeStatus.InVault));
            LowStockItems = ToPersian(db.Products.Count(p => p.Stock <= p.MinStock)) + " کالا";
        }
        catch (Exception ex)
        {
            TotalSalesToday = "خطا";
            TotalRevenueToday = "خطا";
            TotalCustomers = "خطا";
            TotalProducts = "خطا";
            PendingCheques = "خطا";
            LowStockItems = "خطا";
            System.Diagnostics.Debug.WriteLine($"Dashboard error: {ex.Message}");
        }
    }

    private static string ToPersian(long number)
    {
        var persian = new[] { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
        var numStr = number.ToString("N0", System.Globalization.CultureInfo.InvariantCulture);
        return numStr.Aggregate("", (current, ch) => current + (char.IsDigit(ch) ? persian[ch - '0'] : ch));
    }
}
