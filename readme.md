# Multi-Business POS & Accounting Suite

A comprehensive point of sale, inventory, accounting, and customer management software for Iranian businesses.

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12.0-239126?style=for-the-badge&logo=csharp&logoColor=white)
![WPF](https://img.shields.io/badge/WPF-9.0-512BD4?style=for-the-badge&logo=windows&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-3-003B57?style=for-the-badge&logo=sqlite&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Windows](https://img.shields.io/badge/Windows-10%2F11-0078D4?style=for-the-badge&logo=windows&logoColor=white)

---

## Introduction

Professional point of sale and accounting software built with `C#`, `.NET 9`, and `WPF`. Designed for Iranian business environments with complete Persian language support using `Vazirmatn` font, Jalali calendar, VAT calculation, and Tomans rounding. The system works `Offline-First` with `SQLite` database and follows `MVVM` architecture using `CommunityToolkit.Mvvm`.

### Key Features
- POS checkout with product search and multiple payment methods
- Product and category management with icons and subcategories
- Customer management with credit limits and loyalty points
- Supplier debt management with purchase/payment/return tracking
- Customer loyalty system with Bronze/Silver/Gold tiers and point redemption
- Cheque treasury and expense tracking
- Accounting engine with chart of accounts and journal entries
- Barcode scanner support (USB HID and manual entry)
- PDF/Excel export and direct printing
- Global search (Ctrl+G), pagination, and detail views
- 5 color themes (Ocean Blue, Emerald Green, Royal Purple, Sunset Orange, Midnight Dark)
- Font size adjustment and high contrast accessibility
- Responsive layout for all screen sizes

### Technology Stack
- **Runtime:** .NET 9.0 Desktop SDK
- **UI Framework:** WPF with CommunityToolkit.Mvvm
- **Data Access:** EF Core 9 with SQLite (WAL mode)
- **PDF Export:** iTextSharp.LGPLv2.Core
- **Excel Export:** DocumentFormat.OpenXml
- **Barcode:** ZXing.Net
- **Font:** Vazirmatn + Segoe MDL2 Assets (icons)

---

# سیستم مدیریت فروش و حسابداری چندکسب‌وکاره

نرم‌افزار جامع مدیریت فروش، انبار، حسابداری و مشتریان برای کسب‌وکارهای ایرانی

## معرفی

نرم‌افزار حرفه‌ای مدیریت فروشگاه و حسابداری برای کسب‌وکارهای ایرانی ساخته شده با `C#` و `.NET 9` و `WPF`. پشتیبانی کامل از زبان فارسی با فونت `Vazirmatn`، تقویم جلالی، محاسبه مالیات ارزش افزوده و گرد کردن ریالی. سیستم به صورت `Offline-First` با پایگاه داده `SQLite` کار می‌کند و از معماری `MVVM` با `CommunityToolkit.Mvvm` استفاده می‌کند.

### امکانات کلیدی
- صندوق فروشگاهی با جستجوی محصول و روش‌های مختلف پرداخت
- مدیریت کالا و دسته‌بندی با آیکون و زیرمجموعه
- مدیریت مشتریان با سقف اعتبار و امتیاز وفاداری
- مدیریت بدهی تامین‌کنندگان با ردیابی خرید/پرداخت/مرجوعی
- سیستم وفاداری مشتری با سطوح برنزی/نقره‌ای/طلایی و بازخرید امتیاز
- خزانه‌داری چک و ردیابی هزینه‌ها
- موتور حسابداری با دفتر کل و اسناد حسابداری
- پشتیبانی از اسکنر بارکد (USB HID و ورود دستی)
- خروجی PDF/Excel و چاپ مستقیم
- جستجوی سراسری (Ctrl+G)، صفحه‌بندی و نمای جزئیات
- ۵ پوسته رنگی (اقیانوس آبی، سبز زمردی، بنفش سلطنتی، غروب نارنجی، نیمه‌شب تاریک)
- تنظیم اندازه فونت و کنتراست بالا برای دسترسی‌پذیری
- طرح واکنشی برای تمام اندازه‌های صفحه نمایش

### فناوری‌ها
- **محیط اجرایی:** .NET 9.0
- **رابط کاربری:** WPF با CommunityToolkit.Mvvm
- **پایگاه داده:** EF Core 9 با SQLite (حالت WAL)
- **خروجی PDF:** iTextSharp.LGPLv2.Core
- **خروجی Excel:** DocumentFormat.OpenXml
- **بارکد:** ZXing.Net
- **فونت:** Vazirmatn + Segoe MDL2 Assets (آیکون‌ها)
