using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.Services;

/// <summary>
/// Service for managing supplier accounts and debt tracking.
/// Handles purchase recording, payment processing, and balance calculations.
/// </summary>
public class SupplierService
{
    /// <summary>
    /// Create a new supplier account
    /// </summary>
    public Supplier CreateSupplier(string name, string phone, string? address = null, string? contactPerson = null)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        var supplier = new Supplier
        {
            Name = name,
            Phone = phone,
            Address = address,
            ContactPerson = contactPerson,
            TotalDebt = 0,
            TotalPaid = 0
        };
        db.Suppliers.Add(supplier);
        db.SaveChanges();
        return supplier;
    }

    /// <summary>
    /// Get all active suppliers
    /// </summary>
    public List<Supplier> GetAllSuppliers()
    {
        using var db = DatabaseInitializer.CreateDbContext();
        return db.Suppliers.AsNoTracking()
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .ToList();
    }

    /// <summary>
    /// Get a supplier by ID
    /// </summary>
    public Supplier? GetSupplierById(int id)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        return db.Suppliers.Find(id);
    }

    /// <summary>
    /// Record a purchase from supplier - increases supplier debt
    /// </summary>
    public SupplierLedgerEntry RecordPurchase(int supplierId, decimal amount, string description, string? invoiceNumber = null)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        using var transaction = db.Database.BeginTransaction();

        try
        {
            var supplier = db.Suppliers.Find(supplierId)
                ?? throw new InvalidOperationException("Supplier not found");

            // Update supplier debt
            supplier.TotalDebt += amount;

            // Create ledger entry
            var entry = new SupplierLedgerEntry
            {
                SupplierId = supplierId,
                Type = LedgerEntryType.Purchase,
                Amount = amount,
                Description = description,
                InvoiceNumber = invoiceNumber,
                EntryDate = DateTime.Now,
                CreatedAt = DateTime.Now
            };
            db.SupplierLedgerEntries.Add(entry);

            db.SaveChanges();
            transaction.Commit();
            return entry;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    /// <summary>
    /// Record a payment to supplier - reduces supplier debt
    /// </summary>
    public SupplierLedgerEntry RecordPayment(int supplierId, decimal amount, string description)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        using var transaction = db.Database.BeginTransaction();

        try
        {
            var supplier = db.Suppliers.Find(supplierId)
                ?? throw new InvalidOperationException("Supplier not found");

            if (amount > supplier.Balance)
                throw new InvalidOperationException("Payment amount exceeds supplier balance");

            // Update supplier paid amount
            supplier.TotalPaid += amount;

            // Create ledger entry
            var entry = new SupplierLedgerEntry
            {
                SupplierId = supplierId,
                Type = LedgerEntryType.Payment,
                Amount = amount,
                Description = description,
                EntryDate = DateTime.Now,
                CreatedAt = DateTime.Now
            };
            db.SupplierLedgerEntries.Add(entry);

            db.SaveChanges();
            transaction.Commit();
            return entry;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    /// <summary>
    /// Record a return to supplier - reduces supplier debt
    /// </summary>
    public SupplierLedgerEntry RecordReturn(int supplierId, decimal amount, string description, string? invoiceNumber = null)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        using var transaction = db.Database.BeginTransaction();

        try
        {
            var supplier = db.Suppliers.Find(supplierId)
                ?? throw new InvalidOperationException("Supplier not found");

            supplier.TotalDebt -= amount;

            var entry = new SupplierLedgerEntry
            {
                SupplierId = supplierId,
                Type = LedgerEntryType.Return,
                Amount = amount,
                Description = description,
                InvoiceNumber = invoiceNumber,
                EntryDate = DateTime.Now,
                CreatedAt = DateTime.Now
            };
            db.SupplierLedgerEntries.Add(entry);

            db.SaveChanges();
            transaction.Commit();
            return entry;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    /// <summary>
    /// Get all ledger entries for a supplier
    /// </summary>
    public List<SupplierLedgerEntry> GetSupplierLedger(int supplierId)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        return db.SupplierLedgerEntries.AsNoTracking()
            .Where(e => e.SupplierId == supplierId && e.IsActive)
            .OrderByDescending(e => e.EntryDate)
            .ToList();
    }

    /// <summary>
    /// Get total debt across all suppliers
    /// </summary>
    public decimal GetTotalSupplierDebt()
    {
        using var db = DatabaseInitializer.CreateDbContext();
        return db.Suppliers
            .Where(s => s.IsActive)
            .Sum(s => s.TotalDebt - s.TotalPaid);
    }

    /// <summary>
    /// Get suppliers with outstanding debt
    /// </summary>
    public List<Supplier> GetSuppliersWithDebt()
    {
        using var db = DatabaseInitializer.CreateDbContext();
        return db.Suppliers.AsNoTracking()
            .Where(s => s.IsActive && (s.TotalDebt - s.TotalPaid) > 0)
            .ToList()
            .OrderByDescending(s => s.Balance)
            .ToList();
    }

    /// <summary>
    /// Soft delete a supplier
    /// </summary>
    public void DeleteSupplier(int supplierId)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        var supplier = db.Suppliers.Find(supplierId);
        if (supplier != null)
        {
            supplier.IsActive = false;
            db.SaveChanges();
        }
    }
}
