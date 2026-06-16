using Microsoft.EntityFrameworkCore;
using PosAccountingApp.Data;
using PosAccountingApp.Models;

namespace PosAccountingApp.Services;

/// <summary>
/// Service for managing customer loyalty tiers and points.
/// Handles point accumulation, redemption, and tier upgrades.
/// </summary>
public class LoyaltyService
{
    /// <summary>
    /// Calculate points earned from a purchase amount
    /// </summary>
    public int CalculatePoints(decimal purchaseAmount)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        var config = db.Set<LoyaltyConfig>().FirstOrDefault() ?? new LoyaltyConfig();

        if (purchaseAmount <= 0) return 0;
        int rawPoints = (int)(purchaseAmount / 1000) * config.PointsPer1000Tomans;
        return rawPoints;
    }

    /// <summary>
    /// Calculate discount percentage based on customer tier
    /// </summary>
    public decimal CalculateTierDiscount(CustomerTier tier)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        var config = db.Set<LoyaltyConfig>().FirstOrDefault() ?? new LoyaltyConfig();

        return tier switch
        {
            CustomerTier.Bronze => config.BronzeDiscount,
            CustomerTier.Silver => config.SilverDiscount,
            CustomerTier.Gold => config.GoldDiscount,
            _ => 0
        };
    }

    /// <summary>
    /// Determine customer tier based on total purchase amount
    /// </summary>
    public CustomerTier DetermineTier(decimal totalPurchases)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        var config = db.Set<LoyaltyConfig>().FirstOrDefault() ?? new LoyaltyConfig();

        if (totalPurchases >= config.GoldThreshold) return CustomerTier.Gold;
        if (totalPurchases >= config.SilverThreshold) return CustomerTier.Silver;
        return CustomerTier.Bronze;
    }

    /// <summary>
    /// Award points to a customer for a purchase
    /// </summary>
    public LoyaltyTransaction AwardPoints(int customerId, decimal purchaseAmount, int? saleId = null)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        using var transaction = db.Database.BeginTransaction();

        try
        {
            var customer = db.Customers.Find(customerId)
                ?? throw new InvalidOperationException("Customer not found");

            int points = CalculatePoints(purchaseAmount);
            if (points <= 0) return null!;

            customer.LoyaltyPoints += points;

            var tx = new LoyaltyTransaction
            {
                CustomerId = customerId,
                Type = LoyaltyTransactionType.Earn,
                Points = points,
                Description = $"امتیاز خرید به مبلغ {purchaseAmount:N0} ریال",
                SaleId = saleId,
                TransactionDate = DateTime.Now,
                CreatedAt = DateTime.Now
            };
            db.Set<LoyaltyTransaction>().Add(tx);

            // Check and update tier
            var totalPurchases = GetTotalPurchases(customerId);
            var newTier = DetermineTier(totalPurchases + purchaseAmount);

            db.SaveChanges();
            transaction.Commit();
            return tx;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    /// <summary>
    /// Redeem points for discount
    /// </summary>
    public LoyaltyTransaction RedeemPoints(int customerId, int points, string description)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        using var transaction = db.Database.BeginTransaction();

        try
        {
            var customer = db.Customers.Find(customerId)
                ?? throw new InvalidOperationException("Customer not found");

            if (customer.LoyaltyPoints < points)
                throw new InvalidOperationException("Insufficient points");

            customer.LoyaltyPoints -= points;

            var tx = new LoyaltyTransaction
            {
                CustomerId = customerId,
                Type = LoyaltyTransactionType.Redeem,
                Points = -points,
                Description = description,
                TransactionDate = DateTime.Now,
                CreatedAt = DateTime.Now
            };
            db.Set<LoyaltyTransaction>().Add(tx);

            db.SaveChanges();
            transaction.Commit();
            return tx;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    /// <summary>
    /// Get loyalty transaction history for a customer
    /// </summary>
    public List<LoyaltyTransaction> GetTransactionHistory(int customerId)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        return db.Set<LoyaltyTransaction>().AsNoTracking()
            .Where(t => t.CustomerId == customerId && t.IsActive)
            .OrderByDescending(t => t.TransactionDate)
            .ToList();
    }

    /// <summary>
    /// Get total purchase amount for a customer
    /// </summary>
    public decimal GetTotalPurchases(int customerId)
    {
        using var db = DatabaseInitializer.CreateDbContext();
        return db.Sales.AsNoTracking()
            .Where(s => s.CustomerId == customerId && s.IsActive)
            .Sum(s => s.TotalAmount);
    }

    /// <summary>
    /// Get all customers with their tiers
    /// </summary>
    public List<(Customer Customer, CustomerTier Tier, int Points)> GetCustomersWithTiers()
    {
        using var db = DatabaseInitializer.CreateDbContext();
        var customers = db.Customers.AsNoTracking()
            .Where(c => c.IsActive)
            .ToList();

        return customers.Select(c =>
        {
            var totalPurchases = GetTotalPurchases(c.Id);
            var tier = DetermineTier(totalPurchases);
            return (c, tier, c.LoyaltyPoints);
        }).ToList();
    }
}
