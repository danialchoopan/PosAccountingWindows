using System.Windows;
using System.Windows.Media;

namespace PosAccountingApp.Data;

public enum AppTheme { OceanBlue, EmeraldGreen, RoyalPurple, SunsetOrange, MidnightDark }

public static class ThemeManager
{
    private static readonly Dictionary<AppTheme, (Color Bg, Color Surface, Color Sidebar, Color Card,
        Color TextPrimary, Color TextSecondary, Color Accent, Color AccentDark,
        Color Error, Color Success, Color Warning, Color Border,
        Color InputBg, Color InputBorder, Color HoverBg, Color RowAlt,
        Color HeaderBg, Color BadgeBg, Color BadgeFg)> Themes = new()
    {
        [AppTheme.OceanBlue] = (
            Bg: Color.FromRgb(0xF5, 0xF7, 0xFA), Surface: Colors.White,
            Sidebar: Color.FromRgb(0xE8, 0xEE, 0xF4), Card: Colors.White,
            TextPrimary: Color.FromRgb(0x1A, 0x23, 0x32), TextSecondary: Color.FromRgb(0x6B, 0x7B, 0x8D),
            Accent: Color.FromRgb(0x15, 0x65, 0xC0), AccentDark: Color.FromRgb(0x0D, 0x47, 0xA1),
            Error: Color.FromRgb(0xD3, 0x2F, 0x2F), Success: Color.FromRgb(0x2E, 0x7D, 0x32),
            Warning: Color.FromRgb(0xF5, 0x7F, 0x17), Border: Color.FromRgb(0xD6, 0xDE, 0xE8),
            InputBg: Colors.White, InputBorder: Color.FromRgb(0xB0, 0xBE, 0xC5),
            HoverBg: Color.FromRgb(0xF0, 0xF4, 0xF8), RowAlt: Color.FromRgb(0xFA, 0xFB, 0xFC),
            HeaderBg: Color.FromRgb(0xED, 0xF2, 0xF7), BadgeBg: Color.FromRgb(0xE3, 0xF2, 0xFD),
            BadgeFg: Color.FromRgb(0x15, 0x65, 0xC0)),

        [AppTheme.EmeraldGreen] = (
            Bg: Color.FromRgb(0xF0, 0xF8, 0xF0), Surface: Colors.White,
            Sidebar: Color.FromRgb(0xE8, 0xF5, 0xE9), Card: Colors.White,
            TextPrimary: Color.FromRgb(0x1B, 0x2E, 0x1B), TextSecondary: Color.FromRgb(0x5D, 0x7B, 0x5D),
            Accent: Color.FromRgb(0x2E, 0x7D, 0x32), AccentDark: Color.FromRgb(0x1B, 0x5E, 0x20),
            Error: Color.FromRgb(0xC6, 0x28, 0x28), Success: Color.FromRgb(0x1B, 0x5E, 0x20),
            Warning: Color.FromRgb(0xE6, 0x51, 0x00), Border: Color.FromRgb(0xC8, 0xE6, 0xC9),
            InputBg: Colors.White, InputBorder: Color.FromRgb(0xA5, 0xD6, 0xA7),
            HoverBg: Color.FromRgb(0xE8, 0xF5, 0xE9), RowAlt: Color.FromRgb(0xF1, 0xF8, 0xF1),
            HeaderBg: Color.FromRgb(0xE8, 0xF5, 0xE9), BadgeBg: Color.FromRgb(0xE8, 0xF5, 0xE9),
            BadgeFg: Color.FromRgb(0x2E, 0x7D, 0x32)),

        [AppTheme.RoyalPurple] = (
            Bg: Color.FromRgb(0xF5, 0xF0, 0xFA), Surface: Colors.White,
            Sidebar: Color.FromRgb(0xED, 0xDE, 0xF5), Card: Colors.White,
            TextPrimary: Color.FromRgb(0x1A, 0x0A, 0x2E), TextSecondary: Color.FromRgb(0x7B, 0x5E, 0xA7),
            Accent: Color.FromRgb(0x6A, 0x1B, 0x9A), AccentDark: Color.FromRgb(0x4A, 0x14, 0x8C),
            Error: Color.FromRgb(0xC6, 0x28, 0x28), Success: Color.FromRgb(0x2E, 0x7D, 0x32),
            Warning: Color.FromRgb(0xF5, 0x7F, 0x17), Border: Color.FromRgb(0xCE, 0x93, 0xD8),
            InputBg: Colors.White, InputBorder: Color.FromRgb(0xCE, 0x93, 0xD8),
            HoverBg: Color.FromRgb(0xF3, 0xE5, 0xF5), RowAlt: Color.FromRgb(0xFA, 0xF5, 0xFC),
            HeaderBg: Color.FromRgb(0xF3, 0xE5, 0xF5), BadgeBg: Color.FromRgb(0xF3, 0xE5, 0xF5),
            BadgeFg: Color.FromRgb(0x6A, 0x1B, 0x9A)),

        [AppTheme.SunsetOrange] = (
            Bg: Color.FromRgb(0xFE, 0xF7, 0xF0), Surface: Colors.White,
            Sidebar: Color.FromRgb(0xFF, 0xF3, 0xE0), Card: Colors.White,
            TextPrimary: Color.FromRgb(0x3E, 0x27, 0x23), TextSecondary: Color.FromRgb(0x8D, 0x6E, 0x63),
            Accent: Color.FromRgb(0xE6, 0x51, 0x00), AccentDark: Color.FromRgb(0xBF, 0x36, 0x0C),
            Error: Color.FromRgb(0xC6, 0x28, 0x28), Success: Color.FromRgb(0x2E, 0x7D, 0x32),
            Warning: Color.FromRgb(0xE6, 0x51, 0x00), Border: Color.FromRgb(0xFF, 0xCC, 0xBC),
            InputBg: Colors.White, InputBorder: Color.FromRgb(0xBC, 0xAA, 0xA4),
            HoverBg: Color.FromRgb(0xFF, 0xF3, 0xE0), RowAlt: Color.FromRgb(0xFE, 0xF9, 0xF5),
            HeaderBg: Color.FromRgb(0xFF, 0xF3, 0xE0), BadgeBg: Color.FromRgb(0xFF, 0xF3, 0xE0),
            BadgeFg: Color.FromRgb(0xE6, 0x51, 0x00)),

        [AppTheme.MidnightDark] = (
            Bg: Color.FromRgb(0x12, 0x12, 0x20), Surface: Color.FromRgb(0x1E, 0x1E, 0x30),
            Sidebar: Color.FromRgb(0x0D, 0x0D, 0x1A), Card: Color.FromRgb(0x25, 0x25, 0x40),
            TextPrimary: Color.FromRgb(0xE8, 0xE8, 0xF0), TextSecondary: Color.FromRgb(0x88, 0x88, 0xAA),
            Accent: Color.FromRgb(0x5C, 0x6B, 0xC0), AccentDark: Color.FromRgb(0x3F, 0x51, 0xB5),
            Error: Color.FromRgb(0xEF, 0x53, 0x50), Success: Color.FromRgb(0x66, 0xBB, 0x6A),
            Warning: Color.FromRgb(0xFF, 0xA7, 0x26), Border: Color.FromRgb(0x3A, 0x3A, 0x55),
            InputBg: Color.FromRgb(0x2A, 0x2A, 0x45), InputBorder: Color.FromRgb(0x4A, 0x4A, 0x6A),
            HoverBg: Color.FromRgb(0x2E, 0x2E, 0x4A), RowAlt: Color.FromRgb(0x20, 0x20, 0x38),
            HeaderBg: Color.FromRgb(0x1A, 0x1A, 0x30), BadgeBg: Color.FromRgb(0x1E, 0x2A, 0x50),
            BadgeFg: Color.FromRgb(0x79, 0x86, 0xCB))
    };

    public static void ApplyTheme(AppTheme theme)
    {
        if (!Themes.TryGetValue(theme, out var t)) return;
        var app = Application.Current;
        if (app == null) return;

        app.Resources["BgBrush"] = new SolidColorBrush(t.Bg);
        app.Resources["SurfaceBrush"] = new SolidColorBrush(t.Surface);
        app.Resources["SidebarBgBrush"] = new SolidColorBrush(t.Sidebar);
        app.Resources["CardBgBrush"] = new SolidColorBrush(t.Card);
        app.Resources["TextPrimaryBrush"] = new SolidColorBrush(t.TextPrimary);
        app.Resources["TextSecondaryBrush"] = new SolidColorBrush(t.TextSecondary);
        app.Resources["AccentBrush"] = new SolidColorBrush(t.Accent);
        app.Resources["AccentDarkBrush"] = new SolidColorBrush(t.AccentDark);
        app.Resources["ErrorBrush"] = new SolidColorBrush(t.Error);
        app.Resources["SuccessBrush"] = new SolidColorBrush(t.Success);
        app.Resources["WarningBrush"] = new SolidColorBrush(t.Warning);
        app.Resources["BorderBrush"] = new SolidColorBrush(t.Border);
        app.Resources["InputBgBrush"] = new SolidColorBrush(t.InputBg);
        app.Resources["InputBorderBrush"] = new SolidColorBrush(t.InputBorder);
        app.Resources["HoverBgBrush"] = new SolidColorBrush(t.HoverBg);
        app.Resources["RowAltBgBrush"] = new SolidColorBrush(t.RowAlt);
        app.Resources["HeaderBgBrush"] = new SolidColorBrush(t.HeaderBg);
        app.Resources["BadgeBgBrush"] = new SolidColorBrush(t.BadgeBg);
        app.Resources["BadgeFgBrush"] = new SolidColorBrush(t.BadgeFg);
    }

    public static void ApplyThemeByName(string? themeName)
    {
        if (Enum.TryParse<AppTheme>(themeName, out var theme))
            ApplyTheme(theme);
        else
            ApplyTheme(AppTheme.OceanBlue);
    }
}
