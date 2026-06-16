# Technical Documentation

## Project Structure

```
PosAccountingApp/
├── Controls/          Reusable UI controls
├── Converters/        XAML value converters
├── Data/              DbContext, ThemeManager, ExportHelper, DatabaseInitializer
├── Models/            20+ entity models and enums
├── Resources/         Themes.xaml, Styles.xaml
├── Services/          Business logic services (SupplierService, LoyaltyService)
├── ViewModels/        13 MVVM view models
├── Views/             15 XAML views and 5 windows
└── Migrations/        EF Core migrations
```

## Technology Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| .NET SDK | 9.0 | Runtime |
| WPF | 9.0 | UI Framework |
| EF Core | 9.0 | ORM |
| SQLite | - | Database |
| CommunityToolkit.Mvvm | 8.4 | MVVM |
| iTextSharp.LGPLv2.Core | 3.8 | PDF export |
| DocumentFormat.OpenXml | 3.5 | Excel export |
| ZXing.Net | 0.16 | Barcode/QR decoding |

## Architecture

### MVVM Pattern
- Models define data entities with EF Core mappings
- ViewModels use CommunityToolkit.Mvvm (ObservableProperty, RelayCommand)
- Views use DataTemplate binding to ViewModels
- Services handle business logic and data access

### Database
- SQLite with WAL mode for concurrent reads/writes
- Global soft-delete filter on all entities
- Automatic table creation via EnsureCreated()

### Theme System
- 5 color themes defined in Themes.xaml
- ThemeManager applies colors dynamically via DynamicResource
- Theme preference persisted to settings.json

### Test Infrastructure
- xUnit test project with 19 tests
- SQLite test databases with [ThreadStatic] isolation
- Tests cover SupplierService and LoyaltyService

## Key Patterns

### Service Layer
Services encapsulate business logic and database operations:
- SupplierService: CRUD + purchase/payment/return with transactions
- LoyaltyService: Points calculation, tier management, redemption

### Soft Delete
All entities inherit from BaseEntity with IsActive flag. Global query filter:
```csharp
modelBuilder.Entity<T>().HasQueryFilter(e => e.IsActive);
```

### Atomic Transactions
Financial operations use database transactions:
```csharp
using var transaction = db.Database.BeginTransaction();
// ... operations ...
db.SaveChanges();
transaction.Commit();
```

---

# مستندات فنی

## ساختار پروژه

```
PosAccountingApp/
├── Controls/          کنترلهای قابل استفاده مجدد
├── Converters/        مبدلهای XAML
├── Data/              DbContext، ThemeManager، ExportHelper، DatabaseInitializer
├── Models/            ۲۰+ مدل داده و ایلوم
├── Resources/         Themes.xaml، Styles.xaml
├── Services/          سرویس‌های منطق کسب‌وکار
├── ViewModels/        ۱۳ ویومدل MVVM
├── Views/             ۱۵ نمای XAML و ۵ پنجره
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
| ZXing.Net | ۰.۱۶ | رمزگشایی بارکد/QR |

## معماری

### الگوی MVVM
- مدل‌ها موجودیت‌های داده با نگاشت‌های EF Core تعریف می‌کنند
- ویومدل‌ها از CommunityToolkit.Mvvm استفاده می‌کنند
- نماها با DataTemplate به ویومدل‌ها متصل می‌شوند
- سرویس‌ها منطق کسب‌وکار و عملیات داده را مدیریت می‌کنند

### پایگاه داده
- SQLite با حالت WAL برای خواندن/نوشتن همزمان
- فیلتر حذف نرم سراسری روی تمام موجودیت‌ها
- ایجاد خودکار جداول با EnsureCreated()

### سیستم تم
- ۵ پوسته رنگی در Themes.xaml تعریف شده
- ThemeManager رنگ‌ها را به صورت پویا اعمال می‌کند
- انتخاب تم در settings.json ذخیره می‌شود

### زیرساخت تست
- پروژه تست xUnit با ۱۹ تست
- پایگاه داده تست SQLite با ایزولاسیون [ThreadStatic]
- تست‌ها SupplierService و LoyaltyService را پوشش می‌دهند

## الگوهای کلیدی

### لایه سرویس
سرویس‌ها منطق کسب‌وکار و عملیات داده را مدیریت می‌کنند.

### حذف نرم
تمام موجودیت‌ها از BaseEntity با فلگ IsActive ارث‌بری می‌کنند.

### تراکنش‌های اتمیک
عملیات مالی از تراکنش‌های دیتابیس استفاده می‌کنند.
