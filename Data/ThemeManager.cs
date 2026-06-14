using System.Windows;
using System.Windows.Media;

namespace PosAccountingApp.Data;

public static class ThemeManager
{
    public static bool IsDark { get; private set; }

    public static void ApplyTheme(bool dark)
    {
        IsDark = dark;
        var app = Application.Current;
        if (app == null) return;

        var dict = app.Resources.MergedDictionaries[0];
        if (dict == null) return;

        var theme = dark ? dict["DarkTheme"] as ResourceDictionary : dict["LightTheme"] as ResourceDictionary;
        if (theme == null) return;

        app.Resources["BgBrush"] = new SolidColorBrush((Color)theme["BgColor"]);
        app.Resources["SurfaceBrush"] = new SolidColorBrush((Color)theme["SurfaceColor"]);
        app.Resources["SidebarBgBrush"] = new SolidColorBrush((Color)theme["SidebarBg"]);
        app.Resources["CardBgBrush"] = new SolidColorBrush((Color)theme["CardBg"]);
        app.Resources["TextPrimaryBrush"] = new SolidColorBrush((Color)theme["TextPrimaryColor"]);
        app.Resources["TextSecondaryBrush"] = new SolidColorBrush((Color)theme["TextSecondaryColor"]);
        app.Resources["AccentBrush"] = new SolidColorBrush((Color)theme["AccentColor"]);
        app.Resources["AccentDarkBrush"] = new SolidColorBrush((Color)theme["AccentDarkColor"]);
        app.Resources["ErrorBrush"] = new SolidColorBrush((Color)theme["ErrorColor"]);
        app.Resources["SuccessBrush"] = new SolidColorBrush((Color)theme["SuccessColor"]);
        app.Resources["WarningBrush"] = new SolidColorBrush((Color)theme["WarningColor"]);
        app.Resources["BorderBrush"] = new SolidColorBrush((Color)theme["BorderColor"]);
        app.Resources["InputBgBrush"] = new SolidColorBrush((Color)theme["InputBg"]);
        app.Resources["InputBorderBrush"] = new SolidColorBrush((Color)theme["InputBorder"]);
        app.Resources["HoverBgBrush"] = new SolidColorBrush((Color)theme["HoverBg"]);
        app.Resources["RowAltBgBrush"] = new SolidColorBrush((Color)theme["RowAltBg"]);
        app.Resources["HeaderBgBrush"] = new SolidColorBrush((Color)theme["HeaderBg"]);
        app.Resources["BadgeBgBrush"] = new SolidColorBrush((Color)theme["BadgeBg"]);
        app.Resources["BadgeFgBrush"] = new SolidColorBrush((Color)theme["BadgeFg"]);

        // Window backgrounds
        app.Resources["WindowBgBrush"] = app.Resources["BgBrush"];
    }

    public static void Toggle()
    {
        ApplyTheme(!IsDark);
    }
}
