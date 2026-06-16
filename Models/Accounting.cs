using System.ComponentModel.DataAnnotations;

namespace PosAccountingApp.Models;

public enum AccountType { Asset, Liability, Equity, Revenue, Expense }
public enum EntryStatus { Draft, Posted, Reversed }

public class Account : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public AccountType Type { get; set; }
    public int? ParentAccountId { get; set; }
    public decimal Balance { get; set; }
    public string? Description { get; set; }
}

public class JournalEntry : BaseEntity
{
    public string EntryNumber { get; set; } = string.Empty;
    public DateTime EntryDate { get; set; } = DateTime.Now;
    public string Description { get; set; } = string.Empty;
    public EntryStatus Status { get; set; } = EntryStatus.Draft;
    public int UserId { get; set; }
    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<JournalEntryLine> Lines { get; set; } = new();
}

public class JournalEntryLine : BaseEntity
{
    public int JournalEntryId { get; set; }
    public JournalEntry? JournalEntry { get; set; }

    public int AccountId { get; set; }
    public Account? Account { get; set; }

    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public string? Description { get; set; }
}
