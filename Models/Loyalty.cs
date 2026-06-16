using System.ComponentModel.DataAnnotations;

namespace PosAccountingApp.Models;

public enum CustomerTier { Bronze, Silver, Gold }

public class LoyaltyConfig : BaseEntity
{
    public decimal BronzeThreshold { get; set; } = 0;
    public decimal SilverThreshold { get; set; } = 1000000;
    public decimal GoldThreshold { get; set; } = 5000000;

    public int PointsPer1000Tomans { get; set; } = 10;
    public decimal RedemptionRate { get; set; } = 100; // points per 100 tomans discount

    public decimal BronzeDiscount { get; set; } = 0;
    public decimal SilverDiscount { get; set; } = 2;
    public decimal GoldDiscount { get; set; } = 5;
}

public class LoyaltyTransaction : BaseEntity
{
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public LoyaltyTransactionType Type { get; set; }

    public int Points { get; set; }

    public string Description { get; set; } = string.Empty;

    public int? SaleId { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.Now;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public enum LoyaltyTransactionType
{
    Earn,
    Redeem,
    Bonus,
    Adjustment,
    Expired
}
