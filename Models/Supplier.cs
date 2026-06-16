using System.ComponentModel.DataAnnotations;

namespace PosAccountingApp.Models;

public class Supplier : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? Address { get; set; }

    public string? ContactPerson { get; set; }

    public decimal TotalDebt { get; set; }

    public decimal TotalPaid { get; set; }

    public decimal Balance => TotalDebt - TotalPaid;

    public string? Notes { get; set; }
}

public class SupplierLedgerEntry : BaseEntity
{
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public LedgerEntryType Type { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime EntryDate { get; set; } = DateTime.Now;

    public string? InvoiceNumber { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public enum LedgerEntryType
{
    Purchase,
    Payment,
    Adjustment,
    Return
}
