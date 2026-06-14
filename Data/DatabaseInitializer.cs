using System.IO;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Models;

namespace PosAccountingApp.Data;

public static class DatabaseInitializer
{
    private static string GetDbPath()
    {
        var path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PosAccountingApp", "pos_data.db");
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        return path;
    }

    public static AppDbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite($"Data Source={GetDbPath()}");
        return new AppDbContext(optionsBuilder.Options);
    }

    public static void Initialize()
    {
        using var db = CreateDbContext();
        db.Database.EnsureCreated();
    }
}
