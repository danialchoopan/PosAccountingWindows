# مستندات فنی سیستم مدیریت فروش و حسابداری

## معماری کلی

معماری **MVVM** (Model-View-ViewModel) با WPF و CommunityToolkit.Mvvm

```
PosAccountingApp/
├── Models/              # مدل‌های داده + ایلوم‌ها
├── Data/                # EF Core DbContext + مقداردهی اولیه
├── ViewModels/          # ویومدل‌ها (CommunityToolkit.Mvvm)
├── Views/               # نماهای XAML
├── Converters/          # مبدلهای XAML
├── Resources/           # استایل‌ها و تم روشن
├── Migrations/          # مایگریشن‌های EF Core
├── App.xaml             # نقطه ورود
├── .gitignore           # فایل‌های نادیده‌گرفته‌شده گیت
├── readme.md            # معرفی پروژه (فارسی)
├── setup.md             # راهنمای نصب (فارسی)
└── technical.md         # مستندات فنی (فارسی)
```

## فناوری‌ها

| فناوری | نسخه | کاربرد |
|--------|------|--------|
| .NET SDK | ۹.۰ | محیط اجرایی |
| WPF | ۹.۰ | رابط کاربری |
| EF Core | ۹.۰ | ORM |
| SQLite | - | پایگاه داده محلی |
| CommunityToolkit.Mvvm | ۸.۴ | MVVM |
| System.Text.Json | ۹.۰ | JSON |

## جریان راه‌اندازی اولیه

```
برنامه اجرا می‌شود
    │
    ├── settings.json وجود ندارد؟
    │       │
    │       ▼
    │   SetupWindow ──→ اطلاعات فروشگاه + کد عبور مدیر
    │       │
    │       ▼
    │   دیتابیس ایجاد می‌شود + کاربران پیش‌فرض ساخته می‌شوند
    │       │
    │       ▼
    │   LoginWindow ──→ انتخاب کاربر + ورود
    │
    ├── settings.json وجود دارد؟
    │       │
    │       ▼
    │   LoginWindow ──→ انتخاب کاربر + ورود
    │
    └── ورود موفق
            │
            ▼
        MainWindow ──→ داشبورد + سایدبار + تمام بخش‌ها
```

## جدول کاربران پیش‌فرض

| نام | کد عبور | نقش |
|-----|---------|------|
| مدیر سیستم | (در راه‌اندازی انتخاب می‌شود) | SuperAdmin |
| کارمند ۱ | 1111 | Cashier |
| کارمند ۲ | 2222 | Cashier |
| حسابدار | 3333 | Accountant |

## پایگاه داده

### مسیر ذخیره
```
%LOCALAPPDATA%\PosAccountingApp\pos_data.db
```

### موجودیت‌ها (۱۴ جدول)

| موجودیت | توضیح |
|---------|-------|
| User | کاربران با نقش‌ها و کد عبور |
| Warehouse | انبارها |
| Product | کالاها و خدمات |
| Customer | مشتریان |
| CustomerLedger | دفتر معین مشتری |
| Sale | فاکتورهای فروش |
| SaleItem | آیتم‌های فاکتور |
| Cheque | چک‌های دریافتی/پرداختی |
| Expense | هزینه‌ها |
| RealEstateProperty | املاک |
| Vehicle | خودروها |
| InstallmentBook | دفترچه اقساط |
| InstallmentSchedule | جزئیات اقساط |
| SuspendedInvoice | فاکتورهای معلق |
| CashRegister | صندوق |

### فیلتر حذف نرم
تمام جداول از `IsActive` برای حذف منطقی استفاده می‌کنند.

## فرمول‌های محاسباتی

### مالیات
```
TaxAmount = Subtotal × (VATPercentage / 100)
TotalAmount = (Subtotal + TaxAmount) + Rounding + DeliveryFare
```

### کارمزد املاک
```
فروش: Commission = TotalPrice × CommissionPercentage
اجاره/رهن: Commission = (MortgagePrice × 0.0025 + RentPrice) × CommissionPercentage
```

### فروش اقساطی
```
TotalInterest = PrincipalDebt × (MonthlyRate / 100) × InstallmentCount
MonthlyPayment = (PrincipalDebt + TotalInterest) / InstallmentCount
```

## نقش‌های کاربری

| نقش | دسترسی |
|-----|--------|
| SuperAdmin | دسترسی کامل |
| Admin | مدیریت کالا، مشتری، فروش |
| Cashier | فقط صندوق فروش |
| Broker | مشاوره املاک/خودرو |
| Accountant | گزارشات و حسابداری |

## سیستم صفحه‌کلید

| کلید | عملکرد |
|------|--------|
| F1 | صندوق فروشگاهی |
| F2 | مدیریت کالاها |
| F3 | خزانه‌داری و چک |
| F4 | نگهداری فاکتور |
| F9 | گزارشات |

## قوانین زبانی

- **رابط کاربری:** ۱۰۰٪ فارسی با اصطلاحات استاندارد حسابداری ایرانی
- **کدنویسی:** ۱۰۰٪ انگلیسی
- **کامنت‌ها:** انگلیسی با توضیح مفاهیم حسابداری ایرانی
