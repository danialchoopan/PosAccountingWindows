using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PosAccountingApp.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool flag = value is bool b && b;
        if (parameter?.ToString() == "Invert") flag = !flag;
        return flag ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is Visibility v && v == Visibility.Visible;
    }
}

public class DecimalToPersianStringConverter : IValueConverter
{
    private static readonly string[] PersianDigits = ["۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"];

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is decimal d)
        {
            var formatted = d.ToString("N0", CultureInfo.InvariantCulture);
            return formatted.Aggregate("", (current, c) => current + (char.IsDigit(c) ? PersianDigits[c - '0'] : c));
        }
        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class JalaliDateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dt)
        {
            var pc = new System.Globalization.PersianCalendar();
            return $"{pc.GetYear(dt):0000}/{pc.GetMonth(dt):00}/{pc.GetDayOfMonth(dt):00}";
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class EnumToPersianConverter : IValueConverter
{
    private static readonly Dictionary<string, string> PersianNames = new()
    {
        // Units
        ["Number"] = "عدد", ["Weight"] = "وزن", ["Box"] = "جعبه", ["Meter"] = "متر",
        ["Ton"] = "تن", ["Bag"] = "کیسه", ["CubicMeter"] = "مترمکعب",
        // Payment
        ["Cash"] = "نقدی", ["Card"] = "کارتخوان", ["Ledger"] = "نسیه",
        ["Installments"] = "اقساطی", ["Mixed"] = "ترکیبی",
        // Cheque
        ["Receivable"] = "دریافتی", ["Payable"] = "پرداختی",
        ["InVault"] = "در صندوق", ["Passed"] = "وصول شده", ["Returned"] = "برگشتی", ["Transferred"] = "منتقل شده",
        // Property
        ["Apartment"] = "آپارتمان", ["Villa"] = "ویلایی", ["Commercial"] = "تجاری", ["Land"] = "زمین",
        ["Sale"] = "فروش", ["Rent"] = "اجاره", ["Mortgage"] = "رهن",
        ["Available"] = "موجود", ["Archived"] = "بایگانی", ["Dealt"] = "معامله شده",
        // Vehicle
        ["InShowroom"] = "در نمایشگاه", ["Sold"] = "فروخته شده", ["Reserved"] = "رزرو شده",
        // Expense
        ["Rent"] = "اجاره", ["Utilities"] = "قبوض", ["Salary"] = "حقوق",
        ["Supplies"] = "ลูกอม", ["Maintenance"] = "تعمیرات", ["Transport"] = "حمل‌ونقل", ["Other"] = "سایر",
        // Ledger
        ["Charge"] = "شارژ", ["Payment"] = "پرداخت", ["Sale"] = "فروش",
        // Roles
        ["SuperAdmin"] = "مدیر ارشد", ["Admin"] = "مدیر", ["Cashier"] = "صندوقدار",
        ["Broker"] = "مشاور", ["Accountant"] = "حسابدار",
        // Status
        ["Normal"] = "عادی", ["Returned"] = "مرجوعی",
        ["Unpaid"] = "پرداخت نشده", ["PartiallyPaid"] = "پرداخت جزئی", ["FullyPaid"] = "پرداخت کامل", ["Overdue"] = "سررسید گذشته",
        // Business
        ["Supermarket"] = "سوپرمارکت", ["Boutique"] = "پوشاک", ["RealEstate"] = "املاک",
        ["CarDealership"] = "خودرو", ["ConstructionMaterials"] = "مصالح ساختمانی",
        ["Off"] = "بدون گرد", ["FiveHundred"] = "گرد به ۵۰۰", ["Thousand"] = "گرد به ۱۰۰۰",
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Enum e)
            return PersianNames.TryGetValue(e.ToString(), out var name) ? name : e.ToString();
        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
