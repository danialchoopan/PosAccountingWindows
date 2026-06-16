using System.Windows;
using System.Windows.Media;

namespace PosAccountingApp.Data;

public enum AppTheme { OceanBlue, EmeraldGreen, RoyalPurple, SunsetOrange, MidnightDark }

public static class ThemeManager
{
    private static readonly string[] ThemeKeys =
        ["Theme1_OceanBlue", "Theme2_EmeraldGreen", "Theme3_RoyalPurple", "Theme4_SunsetOrange", "Theme5_MidnightDark"];

    public static void ApplyTheme(AppTheme theme)
    {
        var app = Application.Current;
        if (app == null) return;

        var dict = app.Resources.MergedDictionaries[0];
        if (dict == null) return;

        var themeKey = ThemeKeys[(int)theme];
        var themeColors = dict[themeKey] as ResourceDictionary;
        if (themeColors == null) return;

        app.Resources["BgBrush"] = new SolidColorBrush((Color)themeColors["BgColor"]);
        app.Resources["SurfaceBrush"] = new SolidColorBrush((Color)themeColors["SurfaceColor"]);
        app.Resources["SidebarBgBrush"] = new SolidColorBrush((Color)themeColors["SidebarBg"]);
        app.Resources["CardBgBrush"] = new SolidColorBrush((Color)themeColors["CardBg"]);
        app.Resources["TextPrimaryBrush"] = new SolidColorBrush((Color)themeColors["TextPrimaryColor"]);
        app.Resources["TextSecondaryBrush"] = new SolidColorBrush((Color)themeColors["TextSecondaryColor"]);
        app.Resources["AccentBrush"] = new SolidColorBrush((Color)themeColors["AccentColor"]);
        app.Resources["AccentDarkBrush"] = new SolidColorBrush((Color)themeColors["AccentDarkColor"]);
        app.Resources["ErrorBrush"] = new SolidColorBrush((Color)themeColors["ErrorColor"]);
        app.Resources["SuccessBrush"] = new SolidColorBrush((Color)themeColors["SuccessColor"]);
        app.Resources["WarningBrush"] = new SolidColorBrush((Color)themeColors["WarningColor"]);
        app.Resources["BorderBrush"] = new SolidColorBrush((Color)themeColors["BorderColor"]);
        app.Resources["InputBgBrush"] = new SolidColorBrush((Color)themeColors["InputBgColor"]);
        app.Resources["InputBorderBrush"] = new SolidColorBrush((Color)themeColors["InputBorderColor"]);
        app.Resources["HoverBgBrush"] = new SolidColorBrush((Color)themeColors["HoverBg"]);
        app.Resources["RowAltBgBrush"] = new SolidColorBrush((Color)themeColors["RowAltBgColor"]);
        app.Resources["HeaderBgBrush"] = new SolidColorBrush((Color)themeColors["HeaderBgColor"]);
        app.Resources["BadgeBgBrush"] = new SolidColorBrush((Color)themeColors["BadgeBgColor"]);
        app.Resources["BadgeFgBrush"] = new SolidColorBrush((Color)themeColors["BadgeFgColor"]);
    }

    public static void ApplyThemeByName(string themeName)
    {
        if (Enum.TryParse<AppTheme>(themeName, out var theme))
            ApplyTheme(theme);
        else
            ApplyTheme(AppTheme.OceanBlue);
    }
}
