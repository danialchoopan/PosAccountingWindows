using System.ComponentModel.DataAnnotations;

namespace PosAccountingApp.Models;

public class BaseEntity
{
    public int Id { get; set; }
    public bool IsActive { get; set; } = true;
}

public class User : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string PinCodeHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Cashier;
}

public class Warehouse : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Location { get; set; }
}

public class Product : BaseEntity
{
    public string? Barcode { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public UnitType Unit { get; set; } = UnitType.Number;

    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal Stock { get; set; }
    public decimal MinStock { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public string? Color { get; set; }
    public string? Size { get; set; }

    public bool IsLoose { get; set; }

    public int WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
}

public class Customer : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Phone]
    public string Phone { get; set; } = string.Empty;

    public decimal CreditLimit { get; set; }
    public decimal Balance { get; set; }
    public int LoyaltyPoints { get; set; }
    public DateTime? BirthDate { get; set; }
}

public class CustomerLedger : BaseEntity
{
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public int? SaleId { get; set; }

    public LedgerType Type { get; set; }
    public decimal Amount { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class Sale : BaseEntity
{
    public string InvoiceNumber { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User? User { get; set; }

    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal RoundingAmount { get; set; }
    public decimal DeliveryFare { get; set; }

    public string? DriverName { get; set; }
    public string? DriverPhone { get; set; }
    public string? VehiclePlate { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal TotalNetProfit { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
    public decimal CustomerPaid { get; set; }
    public decimal ChangeAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public SaleStatus Status { get; set; } = SaleStatus.Normal;

    public List<SaleItem> Items { get; set; } = new();
}

public class SaleItem : BaseEntity
{
    public int SaleId { get; set; }
    public Sale? Sale { get; set; }

    public int? ProductId { get; set; }
    public Product? Product { get; set; }

    public int? VehicleId { get; set; }

    public string ProductTitle { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal Subtotal { get; set; }
}

public class Cheque : BaseEntity
{
    public string ChequeNumber { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public string PayerName { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public ChequeType Type { get; set; }
    public ChequeStatus Status { get; set; } = ChequeStatus.InVault;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class Expense : BaseEntity
{
    public ExpenseCategory Category { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class RealEstateProperty : BaseEntity
{
    public PropertyType PropertyType { get; set; }
    public DealType DealType { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
    public decimal Area { get; set; }
    public int RoomsCount { get; set; }

    public decimal TotalPrice { get; set; }
    public decimal MortgagePrice { get; set; }
    public decimal RentPrice { get; set; }

    public string OwnerName { get; set; } = string.Empty;
    public string OwnerPhone { get; set; } = string.Empty;

    public PropertyStatus Status { get; set; } = PropertyStatus.Available;
}

public class Vehicle : BaseEntity
{
    [Required]
    public string Brand { get; set; } = string.Empty;

    [Required]
    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }
    public string Color { get; set; } = string.Empty;

    public string ChassisNumber { get; set; } = string.Empty;
    public string EngineNumber { get; set; } = string.Empty;
    public string PlateNumber { get; set; } = string.Empty;

    public long Mileage { get; set; }

    public bool IsConsignment { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }

    public string? OwnerName { get; set; }
    public string? OwnerPhone { get; set; }

    public VehicleStatus Status { get; set; } = VehicleStatus.InShowroom;
}

public class InstallmentBook : BaseEntity
{
    public int SaleId { get; set; }
    public Sale? Sale { get; set; }

    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public decimal TotalAmountWithInterest { get; set; }
    public int InstallmentCount { get; set; }
    public DateTime StartDate { get; set; }

    public List<InstallmentSchedule> Schedules { get; set; } = new();
}

public class InstallmentSchedule : BaseEntity
{
    public int InstallmentBookId { get; set; }
    public InstallmentBook? InstallmentBook { get; set; }

    public int InstallmentIndex { get; set; }
    public DateTime DueDate { get; set; }
    public decimal AmountDue { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime? PaidDate { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
}

public class SuspendedInvoice : BaseEntity
{
    public int UserId { get; set; }
    public int SlotIndex { get; set; }
    public string ItemsJson { get; set; } = "[]";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class CashRegister : BaseEntity
{
    public DateTime Date { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal TotalCashIn { get; set; }
    public decimal TotalCashOut { get; set; }
    public decimal ClosingBalance { get; set; }
    public bool IsClosed { get; set; }
    public DateTime? ClosedAt { get; set; }
}
