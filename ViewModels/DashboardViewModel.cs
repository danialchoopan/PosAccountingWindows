using CommunityToolkit.Mvvm.ComponentModel;

namespace PosAccountingApp.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty] private string _totalSalesToday = "۰";
    [ObservableProperty] private string _totalRevenueToday = "۰ ریال";
    [ObservableProperty] private string _totalCustomers = "۰";
    [ObservableProperty] private string _totalProducts = "۰";
    [ObservableProperty] private string _pendingCheques = "۰";
    [ObservableProperty] private string _lowStockItems = "۰";

    private static string ToPersianNumber(long number)
    {
        var persian = new[] { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
        return number.ToString("N0").Aggregate("", (c, ch) => c + (char.IsDigit(ch) ? persian[ch - '0'] : ch));
    }

    public void LoadData()
    {
        try
        {
            using var db = Data.DatabaseInitializer.CreateDbContext();
            var today = DateTime.Today;
            var salesToday = db.Sales.Where(s => s.CreatedAt >= today).ToList();

            TotalSalesToday = ToPersianNumber(salesToday.Count);
            TotalRevenueToday = ToPersianNumber((long)salesToday.Sum(s => s.TotalAmount)) + " ریال";
            TotalCustomers = ToPersianNumber(db.Customers.Count());
            TotalProducts = ToPersianNumber(db.Products.Count());
            PendingCheques = ToPersianNumber(db.Cheques.Count(c => c.Status == Models.ChequeStatus.InVault));
            LowStockItems = ToPersianNumber(db.Products.Count(p => p.Stock <= p.MinStock));
        }
        catch { }
    }
}
