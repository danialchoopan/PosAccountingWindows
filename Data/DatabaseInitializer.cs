using System.IO;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Models;

namespace PosAccountingApp.Data;

public static class DatabaseInitializer
{
    private static string? _testDbPath;

    private static string GetDbPath()
    {
        if (_testDbPath != null) return _testDbPath;
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

    /// <summary>
    /// Create a fresh in-memory database for testing
    /// </summary>
    public static AppDbContext CreateTestDbContext()
    {
        _testDbPath = Path.Combine(Path.GetTempPath(), $"pos_test_{Guid.NewGuid()}.db");
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite($"Data Source={_testDbPath}");
        var db = new AppDbContext(optionsBuilder.Options);
        db.Database.EnsureCreated();
        return db;
    }

    /// <summary>
    /// Cleanup test database
    /// </summary>
    public static void CleanupTestDb()
    {
        if (_testDbPath != null)
        {
            try
            {
                // Force close any connections
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (File.Exists(_testDbPath))
                    File.Delete(_testDbPath);
            }
            catch { }
            _testDbPath = null;
        }
    }

    public static void Initialize()
    {
        using var db = CreateDbContext();
        db.Database.EnsureCreated();
    }
}
