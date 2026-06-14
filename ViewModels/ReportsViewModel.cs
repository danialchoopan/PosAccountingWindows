using CommunityToolkit.Mvvm.ComponentModel;

namespace PosAccountingApp.ViewModels;

public partial class ReportsViewModel : ObservableObject
{
    [ObservableProperty] private DateTime _reportStartDate = DateTime.Today.AddDays(-30);
    [ObservableProperty] private DateTime _reportEndDate = DateTime.Today;
    [ObservableProperty] private string _selectedReportType = "فروش روزانه";

    public string[] ReportTypes { get; } =
    [
        "فروش روزانه",
        "فروش ماهانه",
        "سود و زیان",
        "موجودی کالا",
        "گزارش مشتریان",
        "گزارش چک‌ها",
        "گزارش هزینه‌ها"
    ];
}
