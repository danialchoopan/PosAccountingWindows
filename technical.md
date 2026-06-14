# مستندات فنی

## ساختار پروژه

```
PosAccountingApp/
├── Controls/              # کنترلهای قابل استفاده مجدد
│   ├── PaginatedDataGrid  # جدول با صفحه‌بندی/خروجی/چاپ
│   └── DetailWindow       # پنجره جزئیات ردیف
├── Converters/            # مبدلهای XAML
├── Data/                  # DbContext + ThemeManager + ExportHelper
├── Models/                # 16 مدل داده
├── Resources/             # Themes.xaml + Styles.xaml
├── ViewModels/            # 11 ویومدل
├── Views/                 # 12 نمای XAML
└── Migrations/            # مایگریشن‌های EF Core
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

## سیستم صفحه‌بندی

- PaginatedDataGrid: کنترل قابل استفاده مجدد
- انتخاب تعداد ردیف: 5، 10، 20، 50، همه
- دکمه‌های: اولین، قبلی، بعدی، آخرین صفحه
- نمایش "صفحه X از Y" و "کل: N"

## خروجی

### PDF (iTextSharp)
- صفحه A4 افقی
- فونت Tahoma/Persian
- هدر آبی با متن سفید
- RTL (راست به چپ)

### Excel (OpenXml)
- فایل .xlsx
- هدر با فونت Bold
- SharedStringTable برای بهینه‌سازی

### چاپ
- PDF موقت در %TEMP%
- باز شدن در برنامه پیش‌فرض PDF

## جدول دسته‌بندی کالاها

| فیلد | توضیح |
|------|-------|
| Name | نام دسته |
| Description | توضیح |
| Icon | آیکون Segoe MDL2 |
| SortOrder | ترتیب نمایش |
| Profile | پروفایل کسب‌وکار |

## جدول کاربران

| نقش | توضیح |
|-----|-------|
| SuperAdmin | مدیر ارشد |
| Admin | مدیر |
| Cashier | صندوقدار |
| Broker | مشاور |
| Accountant | حسابدار |

## فرمول‌های مالیات

```
TaxAmount = Subtotal × (VATPercentage / 100)
TotalAmount = Subtotal + TaxAmount + Rounding + DeliveryFare
```

## فرمول اقساط

```
TotalInterest = Principal × (MonthlyRate/100) × Count
MonthlyPayment = (Principal + TotalInterest) / Count
```

## سیستم تم

- Themes.xaml: تعریف رنگ‌های روشن/تاریک
- ThemeManager.cs: تغییر پویای تم
- DynamicResource در تمام استایل‌ها
- ذخیره انتخاب تم در settings.json

## قوانین زبانی

- رابط کاربری: ۱۰۰٪ فارسی با فونت Vazirmatn
- کدنویسی: ۱۰۰٪ انگلیسی
