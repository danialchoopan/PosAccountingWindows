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
    /// Create a fresh test database with unique name per instance
    /// </summary>
    public static AppDbContext CreateTestDbContext()
    {
        // Use a unique DB name per thread to avoid conflicts between test classes
        var dbPath = Path.Combine(Path.GetTempPath(), $"pos_test_{Thread.CurrentThread.ManagedThreadId}_{Guid.NewGuid():N}.db");
        _testDbPath = dbPath;
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
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
