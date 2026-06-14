using System.IO;
using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Models;

namespace PosAccountingApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerLedger> CustomerLedgers => Set<CustomerLedger>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();
    public DbSet<Cheque> Cheques => Set<Cheque>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<RealEstateProperty> RealEstateProperties => Set<RealEstateProperty>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<InstallmentBook> InstallmentBooks => Set<InstallmentBook>();
    public DbSet<InstallmentSchedule> InstallmentSchedules => Set<InstallmentSchedule>();
    public DbSet<SuspendedInvoice> SuspendedInvoices => Set<SuspendedInvoice>();
    public DbSet<CashRegister> CashRegisters => Set<CashRegister>();
    public DbSet<ProductCategory> Categories => Set<ProductCategory>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PosAccountingApp", "pos_data.db");

            Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Global soft-delete filter for all BaseEntity-derived types
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(SoftDeleteQueryFilter.MakeFilter(entityType.ClrType));
            }
        }

        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Name).IsUnique();
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.HasIndex(p => p.Barcode).IsUnique().HasFilter("[Barcode] IS NOT NULL");
        });

        modelBuilder.Entity<Customer>(e =>
        {
            e.HasIndex(c => c.Phone).IsUnique();
        });

        modelBuilder.Entity<Sale>(e =>
        {
            e.HasIndex(s => s.InvoiceNumber).IsUnique();
        });

        modelBuilder.Entity<Cheque>(e =>
        {
            e.Property(c => c.Amount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Vehicle>(e =>
        {
            e.HasIndex(v => v.ChassisNumber).IsUnique().HasFilter("[ChassisNumber] IS NOT NULL AND [ChassisNumber] != ''");
            e.HasIndex(v => v.EngineNumber).IsUnique().HasFilter("[EngineNumber] IS NOT NULL AND [EngineNumber] != ''");
        });

        // Seed default SuperAdmin user (PIN: 1234)
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Name = "مدیر سیستم",
            PinCodeHash = "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4",
            Role = UserRole.SuperAdmin,
            IsActive = true
        });

        // Seed default warehouse
        modelBuilder.Entity<Warehouse>().HasData(new Warehouse
        {
            Id = 1,
            Name = "انبار مرکزی",
            Location = "محل اصلی فروشگاه",
            IsActive = true
        });
    }
}

// Generates a LINQ expression for the global soft-delete query filter.
// EF Core requires the lambda parameter type to match the exact entity type, not BaseEntity.
internal static class SoftDeleteQueryFilter
{
    internal static System.Linq.Expressions.LambdaExpression MakeFilter(Type entityType)
    {
        var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "e");
        var property = System.Linq.Expressions.Expression.Property(parameter, nameof(BaseEntity.IsActive));
        var condition = System.Linq.Expressions.Expression.Equal(property, System.Linq.Expressions.Expression.Constant(true));
        return System.Linq.Expressions.Expression.Lambda(condition, parameter);
    }
}
