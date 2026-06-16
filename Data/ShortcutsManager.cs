using System.IO;
using System.Text.Json;

namespace PosAccountingApp.Data;

public class AppShortcut
{
    public string Key { get; set; } = "";
    public string Action { get; set; } = "";
    public string Description { get; set; } = "";
}

public static class ShortcutsManager
{
    private static readonly string ShortcutsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "PosAccountingApp", "shortcuts.json");

    private static readonly List<AppShortcut> DefaultShortcuts = new()
    {
        new() { Key = "F1", Action = "POS", Description = "\u0635\u0646\u062F\u0648\u0642 \u0641\u0631\u0648\u0634\u06AF\u0627\u0647\u06CC" },
        new() { Key = "F2", Action = "Products", Description = "\u06A9\u0627\u0644\u0627\u0647\u0627 \u0648 \u062E\u062F\u0645\u0627\u062A" },
        new() { Key = "F3", Action = "Cheques", Description = "\u062E\u0632\u0627\u0646\u0647\u200C\u062F\u0627\u0631\u06CC \u0648 \u0686\u06A9" },
        new() { Key = "F4", Action = "Categories", Description = "\u062F\u0633\u062A\u0647\u200C\u0628\u0646\u062F\u06CC\u200C\u0647\u0627" },
        new() { Key = "F5", Action = "Customers", Description = "\u0645\u0634\u062A\u0631\u06CC\u0627\u0646" },
        new() { Key = "F6", Action = "Inventory", Description = "\u0645\u062F\u06CC\u0631\u06CC\u062A \u0627\u0646\u0628\u0627\u0631" },
        new() { Key = "F7", Action = "Sells", Description = "\u0641\u0631\u0648\u0634\u200C\u0647\u0627" },
        new() { Key = "F8", Action = "Suppliers", Description = "\u062A\u0627\u0645\u06CC\u0646\u200C\u06A9\u0646\u0646\u062F\u06AF\u0627\u0646" },
        new() { Key = "F9", Action = "Reports", Description = "\u06AF\u0632\u0627\u0631\u0634\u0627\u062A" },
        new() { Key = "F10", Action = "Accounting", Description = "\u062D\u0633\u0627\u0628\u062F\u0627\u0631\u06CC" },
        new() { Key = "F11", Action = "Settings", Description = "\u062A\u0636\u06CC\u0645\u06CC\u0645\u0627\u062A" },
        new() { Key = "F12", Action = "Help", Description = "\u0631\u0627\u0647\u0646\u0645\u0627" },
        new() { Key = "Ctrl+G", Action = "GlobalSearch", Description = "\u062C\u0633\u062A\u062C\u0648\u06CC \u0633\u0631\u0627\u0633\u0631\u06CC" },
        new() { Key = "Ctrl+N", Action = "NewProduct", Description = "\u06A9\u0627\u0644\u0627\u06CC \u062C\u062F\u06CC\u062F" },
        new() { Key = "Ctrl+M", Action = "NewCustomer", Description = "\u0645\u0634\u062A\u0631\u06CC \u062C\u062F\u06CC\u062F" },
    };

    public static List<AppShortcut> LoadShortcuts()
    {
        if (File.Exists(ShortcutsPath))
        {
            try
            {
                var json = File.ReadAllText(ShortcutsPath);
                return JsonSerializer.Deserialize<List<AppShortcut>>(json) ?? DefaultShortcuts;
            }
            catch { return DefaultShortcuts; }
        }
        return DefaultShortcuts;
    }

    public static void SaveShortcuts(List<AppShortcut> shortcuts)
    {
        try
        {
            var dir = Path.GetDirectoryName(ShortcutsPath);
            if (dir != null) Directory.CreateDirectory(dir);
            var json = JsonSerializer.Serialize(shortcuts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ShortcutsPath, json);
        }
        catch { }
    }

    public static string? GetAction(string key)
    {
        var shortcuts = LoadShortcuts();
        return shortcuts.FirstOrDefault(s => s.Key == key)?.Action;
    }
}
