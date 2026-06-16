using System.Security.Cryptography;
using System.Text;
using PosAccountingApp.Models;

namespace PosAccountingApp.Data;

public static class SeedData
{
    public static void Seed()
    {
        using var db = DatabaseInitializer.CreateDbContext();
        db.Database.EnsureCreated();

        // Skip if data already exists
        if (db.Products.Any()) return;

        // Seed products
        db.Products.AddRange(
            new Product { Title = "شیر پاستوریزه ۱ لیتری", Barcode = "1001", Category = "لبنیات", Unit = UnitType.Number, PurchasePrice = 45000, SalePrice = 55000, Stock = 120, MinStock = 20, WarehouseId = 1 },
            new Product { Title = "نان بربری", Barcode = "1002", Category = "نان", Unit = UnitType.Number, PurchasePrice = 5000, SalePrice = 8000, Stock = 50, MinStock = 10, WarehouseId = 1 },
            new Product { Title = "برنج ایرانی ۱۰ کیلویی", Barcode = "1003", Category = "خشکبار", Unit = UnitType.Weight, PurchasePrice = 280000, SalePrice = 350000, Stock = 45, MinStock = 10, WarehouseId = 1 },
            new Product { Title = "روغن آفتابگردان ۱ لیتری", Barcode = "1004", Category = "روغن", Unit = UnitType.Number, PurchasePrice = 65000, SalePrice = 82000, Stock = 80, MinStock = 15, WarehouseId = 1 },
            new Product { Title = "شکر ۵ کیلویی", Barcode = "1005", Category = "خشکبار", Unit = UnitType.Weight, PurchasePrice = 85000, SalePrice = 110000, Stock = 60, MinStock = 10, WarehouseId = 1 },
            new Product { Title = "چای ایرانی ۱۰۰ گرمی", Barcode = "1006", Category = "نوشیدنی", Unit = UnitType.Number, PurchasePrice = 35000, SalePrice = 48000, Stock = 200, MinStock = 30, WarehouseId = 1 },
            new Product { Title = "ماکارونی ۵۰۰ گرمی", Barcode = "1007", Category = "خشکبار", Unit = UnitType.Number, PurchasePrice = 22000, SalePrice = 32000, Stock = 150, MinStock = 20, WarehouseId = 1 },
            new Product { Title = "رب گوجه فرنگی ۴۰۰ گرمی", Barcode = "1008", Category = "کنسروجات", Unit = UnitType.Number, PurchasePrice = 28000, SalePrice = 38000, Stock = 90, MinStock = 15, WarehouseId = 1 },
            new Product { Title = "پنیر پاستوریزه ۲۰۰ گرمی", Barcode = "1009", Category = "لبنیات", Unit = UnitType.Number, PurchasePrice = 38000, SalePrice = 52000, Stock = 70, MinStock = 10, WarehouseId = 1 },
            new Product { Title = "مایع ظرفشویی ۱ لیتری", Barcode = "1010", Category = "بهداشتی", Unit = UnitType.Number, PurchasePrice = 42000, SalePrice = 58000, Stock = 100, MinStock = 20, WarehouseId = 1 }
        );

        // Seed customers
        db.Customers.AddRange(
            new Customer { Name = "علی محمدی", Phone = "09121234567", CreditLimit = 5000000, Balance = -1200000, LoyaltyPoints = 350 },
            new Customer { Name = "زهرا رضایی", Phone = "09198765432", CreditLimit = 3000000, Balance = 0, LoyaltyPoints = 800 },
            new Customer { Name = "محمد حسینی", Phone = "09351112233", CreditLimit = 2000000, Balance = -500000, LoyaltyPoints = 150 },
            new Customer { Name = "فاطمه علیزاده", Phone = "09129998877", CreditLimit = 10000000, Balance = 1500000, LoyaltyPoints = 2000 },
            new Customer { Name = "امیر کریمی", Phone = "09365554433", CreditLimit = 4000000, Balance = 0, LoyaltyPoints = 500 }
        );

        // Seed cheques
        db.Cheques.AddRange(
            new Cheque { ChequeNumber = "1234567", BankName = "ملت", Branch = "تهران مرکز", Amount = 2000000, DueDate = DateTime.Now.AddDays(15), PayerName = "علی محمدی", ReceiverName = "فروشگاه", Type = ChequeType.Receivable, Status = ChequeStatus.InVault },
            new Cheque { ChequeNumber = "9876543", BankName = "صادرات", Branch = "شیراز", Amount = 1500000, DueDate = DateTime.Now.AddDays(30), PayerName = "فروشگاه", ReceiverName = "زهرا رضایی", Type = ChequeType.Payable, Status = ChequeStatus.InVault },
            new Cheque { ChequeNumber = "5555555", BankName = "پاسارگاد", Branch = "اصفهان", Amount = 3000000, DueDate = DateTime.Now.AddDays(-5), PayerName = "محمد حسینی", ReceiverName = "فروشگاه", Type = ChequeType.Receivable, Status = ChequeStatus.Passed }
        );

        // Seed expenses
        db.Expenses.AddRange(
            new Expense { Category = ExpenseCategory.Rent, Description = "اجاره ماهانه فروشگاه", Amount = 15000000, Date = DateTime.Now.AddDays(-10) },
            new Expense { Category = ExpenseCategory.Utilities, Description = "قبض برق ماه گذشته", Amount = 2500000, Date = DateTime.Now.AddDays(-15) },
            new Expense { Category = ExpenseCategory.Salary, Description = "حقوق کارمند فروردین", Amount = 25000000, Date = DateTime.Now.AddDays(-5) },
            new Expense { Category = ExpenseCategory.Transport, Description = "هزینه حمل بار", Amount = 800000, Date = DateTime.Now.AddDays(-3) }
        );

        // Seed suppliers
        db.Suppliers.AddRange(
            new Supplier { Name = "پخش سراسری", Phone = "02188776655", Address = "تهران، بازار بزرگ", ContactPerson = "آقای احمدی", TotalDebt = 45000000, TotalPaid = 30000000 },
            new Supplier { Name = " لبنیات شمال", Phone = "01133445566", Address = "ساری، منطقه صنعتی", ContactPerson = "خانم رستمی", TotalDebt = 12000000, TotalPaid = 12000000 },
            new Supplier { Name = "خشکبار اصفهان", Phone = "03112345678", Address = "اصفهان، خیابان چهارباغ", ContactPerson = "آقای نوری", TotalDebt = 8000000, TotalPaid = 5000000 }
        );

        db.SaveChanges();
    }

    private static string ComputeSha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
