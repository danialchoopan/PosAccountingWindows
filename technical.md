# مستندات فنی

## ساختار پروژه

```
PosAccountingApp/
├── Controls/          کنترلهای قابل استفاده مجدد
│   ├── PaginatedDataGrid   جدول با صفحه‌بندی/خروجی/چاپ
│   └── DetailWindow        پنجره جزئیات ردیف
├── Converters/        مبدلهای XAML
├── Data/              DbContext + ThemeManager + ExportHelper
├── Models/            ۱۶ مدل داده + ۱۶ ایلوم
├── Resources/         Themes.xaml + Styles.xaml
├── ViewModels/        ۱۱ ویومدل
├── Views/             ۱۲ نمای XAML
└── Migrations/        مایگریشن‌های EF Core
```

## فناوری‌ها

| فناوری | نسخه | کاربرد |
|--------|------|--------|
| .NET SDK | ۹.۰ | محیط اجرایی |
| WPF | ۹.۰ | رابط کاربری |
| EF Core | ۹.۰ | ORM |
| SQLite | - | پایگاه داده |
| CommunityToolkit.Mvvm | ۸.۴ | MVVM |
| iTextSharp.LGPLv2.Core | ۳.۸ | خروجی PDF |
| DocumentFormat.OpenXml | ۳.۵ | خروجی Excel |

## جریان راه‌اندازی

```
برنامه اجرا → settings.json وجود ندارد → SetupWindow → LoginWindow → MainWindow
برنامه اجرا → settings.json وجود دارد → LoginWindow → MainWindow
```

## پایگاه داده

### مسیر: `%LOCALAPPDATA%\PosAccountingApp\pos_data.db`

### ۱۶ جدول
User, Warehouse, Product, ProductCategory, Customer, CustomerLedger,
Sale, SaleItem, Cheque, Expense, RealEstateProperty, Vehicle,
InstallmentBook, InstallmentSchedule, SuspendedInvoice, CashRegister

### فیلتر حذف نرم
تمام جداول از `IsActive` استفاده می‌کنند.

## سیستم صفحه‌بندی

- PaginatedDataGrid: کنترل قابل استفاده مجدد
- انتخاب تعداد ردیف: ۵، ۱۰، ۲۰، ۵۰، همه
- مخفی شدن هنگام خالی بودن جدول
- نمایش پیام خالی بودن

## خروجی

### PDF (iTextSharp)
- صفحه A4 افقی
- فونت Tahoma/Persian
- RTL

### Excel (OpenXml)
- فایل .xlsx
- SharedStringTable

### چاپ
- PDF موقت در %TEMP%

## سیستم تم

- Themes.xaml: تعریف رنگ‌های روشن/تاریک
- ThemeManager.cs: تغییر پویای تم
- DynamicResource در تمام استایل‌ها

## دسترسی‌پذیری

- اندازه فونت: ۱۰ تا ۲۰
- کنتراست بالا: متن مشکی روی زمینه سفید

## قوانین زبانی

- رابط کاربری: ۱۰۰٪ فارسی با فونت Vazirmatn
- کدنویسی: ۱۰۰٪ انگلیسی

## آمار پروژه

- ۲۰+ commit
- ۶۰+ فایل
- ۷۰۰۰+ خط کد
- ۱۰ تست xUnit

## ماژول جدید: مدیریت بدهی تامین‌کنندگان

### مدل‌ها
- `Supplier` - اطلاعات تامین‌کننده (نام، تلفن، آدرس، بدهی کل، پرداختی کل)
- `SupplierLedgerEntry` - تاریخچه تراکنش‌ها (خرید، پرداخت، مرجوعی، تعدیل)
- `LedgerEntryType` - ایلوم نوع تراکنش

### سرویس‌ها
- `SupplierService` - CRUD + عملیات مالی با تراکنش اتمیک

### تست‌ها
- ۱۰ تست xUnit پوشش‌دهنده تمام عملیات
