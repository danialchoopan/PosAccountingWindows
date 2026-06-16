using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PosAccountingApp.Data;
using PosAccountingApp.Models;
using PosAccountingApp.Services;

namespace PosAccountingApp.ViewModels;

public partial class LoyaltyViewModel : ObservableObject
{
    private readonly LoyaltyService _loyaltyService = new();

    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private Customer? _selectedCustomer;
    [ObservableProperty] private CustomerTier _selectedTier;
    [ObservableProperty] private int _customerPoints;
    [ObservableProperty] private decimal _customerTotalPurchases;
    [ObservableProperty] private decimal _tierDiscount;

    [ObservableProperty] private int _redeemPointsAmount;
    [ObservableProperty] private string _redeemDescription = string.Empty;

    public ObservableCollection<CustomerTierInfo> CustomerTiers { get; } = new();
    public ObservableCollection<LoyaltyTransaction> Transactions { get; } = new();

    public LoyaltyViewModel() { LoadData(); }

    public void LoadData()
    {
        var data = _loyaltyService.GetCustomersWithTiers();
        CustomerTiers.Clear();
        foreach (var (customer, tier, points) in data)
        {
            CustomerTiers.Add(new CustomerTierInfo
            {
                Customer = customer,
                Tier = tier,
                Points = points,
                TotalPurchases = _loyaltyService.GetTotalPurchases(customer.Id),
                Discount = _loyaltyService.CalculateTierDiscount(tier)
            });
        }
    }

    [RelayCommand]
    private void ClearSearch() { SearchText = string.Empty; }

    [RelayCommand]
    private void SelectCustomer(CustomerTierInfo? info)
    {
        if (info == null) return;
        SelectedCustomer = info.Customer;
        SelectedTier = info.Tier;
        CustomerPoints = info.Points;
        CustomerTotalPurchases = info.TotalPurchases;
        TierDiscount = info.Discount;

        var history = _loyaltyService.GetTransactionHistory(info.Customer.Id);
        Transactions.Clear();
        foreach (var t in history) Transactions.Add(t);
    }

    [RelayCommand]
    private void RedeemPoints()
    {
        if (SelectedCustomer == null || RedeemPointsAmount <= 0) return;
        _loyaltyService.RedeemPoints(SelectedCustomer.Id, RedeemPointsAmount,
            string.IsNullOrEmpty(RedeemDescription) ? "استفاده از امتیاز" : RedeemDescription);
        RedeemPointsAmount = 0;
        RedeemDescription = string.Empty;
        LoadData();
    }

    [RelayCommand]
    private void AwardBonus(int points)
    {
        if (SelectedCustomer == null || points <= 0) return;
        using var db = DatabaseInitializer.CreateDbContext();
        var customer = db.Customers.Find(SelectedCustomer.Id);
        if (customer != null)
        {
            customer.LoyaltyPoints += points;
            var tx = new LoyaltyTransaction
            {
                CustomerId = SelectedCustomer.Id,
                Type = LoyaltyTransactionType.Bonus,
                Points = points,
                Description = $"امتیاز جایزه: {points} امتیاز",
                TransactionDate = DateTime.Now,
                CreatedAt = DateTime.Now
            };
            db.Set<LoyaltyTransaction>().Add(tx);
            db.SaveChanges();
        }
        LoadData();
    }
}

public class CustomerTierInfo
{
    public Customer Customer { get; set; } = null!;
    public CustomerTier Tier { get; set; }
    public int Points { get; set; }
    public decimal TotalPurchases { get; set; }
    public decimal Discount { get; set; }
}
