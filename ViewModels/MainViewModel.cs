using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PosAccountingApp.Models;

namespace PosAccountingApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private ObservableObject _currentView;
    [ObservableProperty] private string _currentViewTitle = "داشبورد";
    [ObservableProperty] private string _currentUser = "مدیر";
    [ObservableProperty] private bool _isDarkTheme;
    [ObservableProperty] private string _statusMessage = "آماده";

    public DashboardViewModel DashboardVm { get; } = new();
    public ProductsViewModel ProductsVm { get; } = new();
    public CustomersViewModel CustomersVm { get; } = new();
    public PosViewModel PosVm { get; } = new();
    public ChequesViewModel ChequesVm { get; } = new();
    public ExpensesViewModel ExpensesVm { get; } = new();
    public ReportsViewModel ReportsVm { get; } = new();
    public SettingsViewModel SettingsVm { get; } = new();
    public UsersViewModel UsersVm { get; } = new();
    public CategoriesViewModel CategoriesVm { get; } = new();
    public AccountingViewModel AccountingVm { get; } = new();
    public SupplierViewModel SupplierVm { get; } = new();
    public InventoryViewModel InventoryVm { get; } = new();
    public SellsViewModel SellsVm { get; } = new();

    public bool IsAdmin => AppSettings.CurrentUser?.Role == UserRole.SuperAdmin
                        || AppSettings.CurrentUser?.Role == UserRole.Admin;

    public MainViewModel()
    {
        _currentView = DashboardVm;

        if (AppSettings.CurrentUser != null)
        {
            var roleNames = new Dictionary<UserRole, string>
            {
                { UserRole.SuperAdmin, "مدیر ارشد" },
                { UserRole.Admin, "مدیر" },
                { UserRole.Cashier, "صندوقدار" },
                { UserRole.Broker, "مشاور" },
                { UserRole.Accountant, "حسابدار" }
            };
            CurrentUser = AppSettings.CurrentUser.Name;
            if (roleNames.TryGetValue(AppSettings.CurrentUser.Role, out var r))
                StatusMessage = r;
        }

        // Load saved theme
        var settings = AppSettings.Load();
        Data.ThemeManager.ApplyThemeByName(settings.SelectedTheme);

        DashboardVm.LoadData();
    }

    [RelayCommand]
    private void NavigateTo(string viewName)
    {
        switch (viewName)
        {
            case "Dashboard":
                CurrentView = DashboardVm; CurrentViewTitle = "داشبورد";
                DashboardVm.LoadData(); break;
            case "Products":
                CurrentView = ProductsVm; CurrentViewTitle = "کالاها و خدمات";
                ProductsVm.LoadProducts(); break;
            case "Customers":
                CurrentView = CustomersVm; CurrentViewTitle = "مشتریان";
                CustomersVm.LoadCustomers(); break;
            case "POS":
                CurrentView = PosVm; CurrentViewTitle = "صندوق فروشگاهی"; break;
            case "Cheques":
                CurrentView = ChequesVm; CurrentViewTitle = "خزانه‌داری و چک";
                ChequesVm.LoadCheques(); break;
            case "Expenses":
                CurrentView = ExpensesVm; CurrentViewTitle = "هزینه‌ها";
                ExpensesVm.LoadExpenses(); break;
            case "Reports":
                CurrentView = ReportsVm; CurrentViewTitle = "گزارشات"; break;
            case "Users":
                CurrentView = UsersVm; CurrentViewTitle = "مدیریت کاربران";
                UsersVm.LoadUsers(); break;
            case "Categories":
                CurrentView = CategoriesVm; CurrentViewTitle = "دسته‌بندی کالاها";
                CategoriesVm.LoadCategories(); break;
            case "Accounting":
                CurrentView = AccountingVm; CurrentViewTitle = "حسابداری";
                AccountingVm.LoadData(); break;
            case "Suppliers":
                CurrentView = SupplierVm; CurrentViewTitle = "\u062A\u0627\u0645\u06CC\u0646\u200C\u06A9\u0646\u0646\u062F\u06AF\u0627\u0646";
                SupplierVm.LoadSuppliers(); break;
            case "Inventory":
                CurrentView = InventoryVm; CurrentViewTitle = "\u0645\u062F\u06CC\u0631\u06CC\u062A \u0627\u0646\u0628\u0627\u0631";
                InventoryVm.LoadData(); break;
            case "Sells":
                CurrentView = SellsVm; CurrentViewTitle = "\u0641\u0631\u0648\u0634\u200C\u0647\u0627";
                SellsVm.LoadData(); break;
            case "Settings":
                CurrentView = SettingsVm; CurrentViewTitle = "تنظیمات"; break;
        }
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        // Theme switching is now handled in SettingsViewModel
    }

    [RelayCommand]
    private void OpenGlobalSearch()
    {
        var window = new Views.GlobalSearchWindow();
        window.ShowDialog();
    }

    [RelayCommand]
    private void OpenBarcodeScanner()
    {
        var scanner = new Views.BarcodeScannerWindow();
        scanner.BarcodeScanned += code =>
        {
            // If POS is active, send the scanned code to POS
            if (CurrentView is PosViewModel posVm)
            {
                posVm.BarcodeInput = code;
                posVm.AddByBarcodeCommand.Execute(null);
            }
        };
        scanner.ShowDialog();
    }
}
