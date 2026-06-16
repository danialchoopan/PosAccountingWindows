# Installation & Setup Guide

## Prerequisites

- Windows 10 or 11
- .NET 9 SDK

## Installation

```bash
winget install Microsoft.DotNet.SDK.9
cd PosAccountingApp
dotnet run
```

## First Run

### Step 1: Initial Setup
Enter your shop information: name, business type, phone, address, admin PIN, and VAT percentage. Click "Start Working".

### Step 2: Login
Select your user from the dropdown and enter your PIN code.

### Step 3: Start
You will be taken to the dashboard with all features available.

## Adding Users
Click "User Management" in the sidebar to add new users with different roles (Admin, Cashier, Accountant, Broker).

## Adding Categories
Click "Categories" in the sidebar. You can create parent categories and subcategories with icons.

## Adding Products
Click "Products" and then "New Product". Select category from dropdown and unit from the Farsi list.

## Search
Press Ctrl+G for global search across all sections.

## Export and Print
On any page with a data table:
- PDF button: Export to PDF
- Excel button: Export to Excel
- Print button: Direct print
- Dropdown: Select rows per page (5/10/20/50/All)

## Themes
Go to Settings to choose from 5 color themes:
1. Ocean Blue (default)
2. Emerald Green
3. Royal Purple
4. Sunset Orange
5. Midnight Dark

## Reset
Delete these files and restart:
```
%LOCALAPPDATA%\PosAccountingApp\pos_data.db
%LOCALAPPDATA%\PosAccountingApp\settings.json
```

---

# راهنمای نصب و راه‌اندازی

## پیش‌نیازها

- ویندوز ۱۰ یا ۱۱
- .NET 9 SDK

## نصب

```bash
winget install Microsoft.DotNet.SDK.9
cd PosAccountingApp
dotnet run
```

## اولین اجرا

### مرحله ۱: تنظیم اولیه
اطلاعات فروشگاه را وارد کنید: نام، نوع کسب‌وکار، تلفن، آدرس، کد عبور مدیر و درصد مالیات. روی "شروع به کار" کلیک کنید.

### مرحله ۲: ورود
کاربر خود را از لیست انتخاب کرده و کد عبور را وارد کنید.

### مرحله ۳: شروع
وارد داشبورد با تمام امکانات می‌شوید.

## اضافه کردن کاربر
از سایدبار روی "مدیریت کاربران" کلیک کنید تا کاربران جدید با نقش‌های مختلف بسازید.

## اضافه کردن دسته‌بندی
از سایدبار روی "دسته‌بندی‌ها" کلیک کنید. می‌توانید دسته‌های والد و زیرمجموعه با آیکون بسازید.

## اضافه کردن کالا
روی "کالاها" و سپس "کالای جدید" کلیک کنید. دسته را از لیست کشویی و واحد را از لیست فارسی انتخاب کنید.

## جستجو
کلیدهای Ctrl+G برای جستجوی سراسری در تمام بخش‌ها.

## خروجی و چاپ
در هر صفحه با جدول داده:
- دکمه PDF: خروجی PDF
- دکمه Excel: خروجی اکسل
- دکمه چاپ: چاپ مستقیم
- کشویی: انتخاب تعداد ردیف در صفحه (۵/۱۰/۲۰/۵۰/همه)

## پوسته‌ها
از صفحه تنظیمات یکی از ۵ پوسته رنگی را انتخاب کنید:
۱. اقیانوس آبی (پیش‌فرض)
۲. سبز زمردی
۳. بنفش سلطنتی
۴. غروب نارنجی
۵. نیمه‌شب تاریک

## ریست
این فایل‌ها را حذف کرده و برنامه را مجدداً اجرا کنید:
```
%LOCALAPPDATA%\PosAccountingApp\pos_data.db
%LOCALAPPDATA%\PosAccountingApp\settings.json
```
