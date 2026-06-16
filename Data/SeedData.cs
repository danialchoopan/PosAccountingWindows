using PosAccountingApp.Models;

namespace PosAccountingApp.Data;

public static class SeedData
{
    public static void Seed()
    {
        using var db = DatabaseInitializer.CreateDbContext();
        db.Database.EnsureCreated();
        if (db.Products.Any()) return;

        // Categories
        db.Categories.AddRange(
            new ProductCategory { Name = "\u0644\u0628\u0646\u06CC\u0627\u062A", Icon = "\uE7F3", SortOrder = 1 },
            new ProductCategory { Name = "\u0646\u0627\u0646", Icon = "\uE74C", SortOrder = 2 },
            new ProductCategory { Name = "\u062E\u0634\u06A9\u0628\u0627\u0631", Icon = "\uE787", SortOrder = 3 },
            new ProductCategory { Name = "\u0631\u0648\u063A\u0646", Icon = "\uE7C3", SortOrder = 4 },
            new ProductCategory { Name = "\u0646\u0648\u0634\u06CC\u062F\u0646\u06CC", Icon = "\uE774", SortOrder = 5 },
            new ProductCategory { Name = "\u06A9\u0646\u0633\u0631\u0648\u062C\u0627\u062A", Icon = "\uE74D", SortOrder = 6 },
            new ProductCategory { Name = "\u0628\u0647\u062F\u0627\u0634\u062A\u06CC", Icon = "\uE7BA", SortOrder = 7 },
            new ProductCategory { Name = "\u067E\u0631\u0648\u0638\u0646\u06CC", Icon = "\uE716", SortOrder = 8 },
            new ProductCategory { Name = "\u062A\u0635\u0648\u06CC\u0631\u06CC", Icon = "\uE8D8", SortOrder = 9 }
        );

        // Products (10 items with categories)
        db.Products.AddRange(
            new Product { Title = "\u0634\u06CC\u0631 \u0641\u0627\u0633\u062A\u0648\u0631\u06CC\u0632\u0647 \u06F1 \u0644\u06CC\u062A\u0631\u06CC", Barcode = "1001", Category = "\u0644\u0628\u0646\u06CC\u0627\u062A", Unit = UnitType.Number, PurchasePrice = 45000, SalePrice = 55000, Stock = 120, MinStock = 20, WarehouseId = 1 },
            new Product { Title = "\u0646\u0627\u0646 \u0628\u0631\u0628\u0631\u06CC", Barcode = "1002", Category = "\u0646\u0627\u0646", Unit = UnitType.Number, PurchasePrice = 5000, SalePrice = 8000, Stock = 50, MinStock = 10, WarehouseId = 1 },
            new Product { Title = "\u0628\u0631\u0646\u062C \u0627\u06CC\u0631\u0627\u0646\u06CC \u06F1\u06F0 \u06A9\u06CC\u0644\u0648\u06CC", Barcode = "1003", Category = "\u062E\u0634\u06A9\u0628\u0627\u0631", Unit = UnitType.Weight, PurchasePrice = 280000, SalePrice = 350000, Stock = 45, MinStock = 10, WarehouseId = 1 },
            new Product { Title = "\u0631\u0648\u063A\u0646 \u0622\u0641\u062A\u0627\u0628\u06AF\u0631\u062F\u0627\u0646 \u06F1 \u0644\u06CC\u062A\u0631\u06CC", Barcode = "1004", Category = "\u0631\u0648\u063A\u0646", Unit = UnitType.Number, PurchasePrice = 65000, SalePrice = 82000, Stock = 80, MinStock = 15, WarehouseId = 1 },
            new Product { Title = "\u0634\u06A9\u0631 \u06F5 \u06A9\u06CC\u0644\u0648\u06CC", Barcode = "1005", Category = "\u062E\u0634\u06A9\u0628\u0627\u0631", Unit = UnitType.Weight, PurchasePrice = 85000, SalePrice = 110000, Stock = 60, MinStock = 10, WarehouseId = 1 },
            new Product { Title = "\u0686\u0627\u06CC \u0627\u06CC\u0631\u0627\u0646\u06CC \u06F1\u06F0\u06F0 \u06AF\u0631\u0645\u06CC", Barcode = "1006", Category = "\u0646\u0648\u0634\u06CC\u062F\u0646\u06CC", Unit = UnitType.Number, PurchasePrice = 35000, SalePrice = 48000, Stock = 200, MinStock = 30, WarehouseId = 1 },
            new Product { Title = "\u0645\u0627\u06A9\u0627\u0631\u0648\u0646\u06CC \u06F5\u06F0\u06F0 \u06AF\u0631\u0645\u06CC", Barcode = "1007", Category = "\u062E\u0634\u06A9\u0628\u0627\u0631", Unit = UnitType.Number, PurchasePrice = 22000, SalePrice = 32000, Stock = 150, MinStock = 20, WarehouseId = 1 },
            new Product { Title = "\u0631\u0628 \u06AF\u0648\u062C\u0647 \u06F4\u06F0\u06F0 \u06AF\u0631\u0645\u06CC", Barcode = "1008", Category = "\u06A9\u0646\u0633\u0631\u0648\u062C\u0627\u062A", Unit = UnitType.Number, PurchasePrice = 28000, SalePrice = 38000, Stock = 90, MinStock = 15, WarehouseId = 1 },
            new Product { Title = "\u067E\u0646\u06CC\u0631 \u06F2\u06F0\u06F0 \u06AF\u0631\u0645\u06CC", Barcode = "1009", Category = "\u0644\u0628\u0646\u06CC\u0627\u062A", Unit = UnitType.Number, PurchasePrice = 38000, SalePrice = 52000, Stock = 70, MinStock = 10, WarehouseId = 1 },
            new Product { Title = "\u0645\u0627\u06CC\u0639 \u0638\u0631\u0641\u0634\u0648\u06CC\u06CC \u06F1 \u0644\u06CC\u062A\u0631\u06CC", Barcode = "1010", Category = "\u0628\u0647\u062F\u0627\u0634\u062A\u06CC", Unit = UnitType.Number, PurchasePrice = 42000, SalePrice = 58000, Stock = 100, MinStock = 20, WarehouseId = 1 }
        );

        // Customers
        db.Customers.AddRange(
            new Customer { Name = "\u0639\u0644\u06CC \u0645\u062D\u0645\u062F\u06CC", Phone = "09121234567", CreditLimit = 5000000, Balance = -1200000, LoyaltyPoints = 350 },
            new Customer { Name = "\u0632\u0647\u0631\u0627 \u0631\u0636\u0627\u06CC\u06CC", Phone = "09198765432", CreditLimit = 3000000, Balance = 0, LoyaltyPoints = 800 },
            new Customer { Name = "\u0645\u062D\u0645\u062F \u062D\u0633\u06CC\u0646\u06CC", Phone = "09351112233", CreditLimit = 2000000, Balance = -500000, LoyaltyPoints = 150 },
            new Customer { Name = "\u0641\u0627\u0637\u0645\u0647 \u0639\u0644\u06CC\u0632\u0627\u062F\u0647", Phone = "09129998877", CreditLimit = 10000000, Balance = 1500000, LoyaltyPoints = 2000 },
            new Customer { Name = "\u0627\u0645\u06CC\u0631 \u06A9\u0631\u06CC\u0645\u06CC", Phone = "09365554433", CreditLimit = 4000000, Balance = 0, LoyaltyPoints = 500 }
        );

        // Sales with items
        var sale1 = new Sale
        {
            InvoiceNumber = "INV-20260101-001", UserId = 1, CustomerId = 1,
            Subtotal = 166000, TaxAmount = 16600, PaymentMethod = PaymentMethod.Cash,
            CustomerPaid = 200000, ChangeAmount = 17400, CreatedAt = DateTime.Now.AddDays(-5),
            Items = new List<SaleItem>
            {
                new SaleItem { ProductId = 1, ProductTitle = "\u0634\u06CC\u0631", Quantity = 2, UnitPrice = 55000, PurchasePrice = 45000, Subtotal = 110000 },
                new SaleItem { ProductId = 6, ProductTitle = "\u0686\u0627\u06CC", Quantity = 1, UnitPrice = 48000, PurchasePrice = 35000, Subtotal = 48000 }
            }
        };
        sale1.TotalAmount = sale1.Items.Sum(i => i.Subtotal) + sale1.TaxAmount;
        sale1.TotalNetProfit = sale1.Items.Sum(i => (i.UnitPrice - i.PurchasePrice) * i.Quantity);

        var sale2 = new Sale
        {
            InvoiceNumber = "INV-20260102-002", UserId = 1, CustomerId = 2,
            Subtotal = 200000, TaxAmount = 20000, PaymentMethod = PaymentMethod.Card,
            CustomerPaid = 220000, ChangeAmount = 0, CreatedAt = DateTime.Now.AddDays(-3),
            Items = new List<SaleItem>
            {
                new SaleItem { ProductId = 3, ProductTitle = "\u0628\u0631\u0646\u062C", Quantity = 1, UnitPrice = 350000, PurchasePrice = 280000, Subtotal = 350000 },
                new SaleItem { ProductId = 2, ProductTitle = "\u0646\u0627\u0646", Quantity = 3, UnitPrice = 8000, PurchasePrice = 5000, Subtotal = 24000 }
            }
        };
        sale2.TotalAmount = sale2.Items.Sum(i => i.Subtotal) + sale2.TaxAmount;
        sale2.TotalNetProfit = sale2.Items.Sum(i => (i.UnitPrice - i.PurchasePrice) * i.Quantity);
        db.Sales.AddRange(sale1, sale2);

        // Cheques
        db.Cheques.AddRange(
            new Cheque { ChequeNumber = "1234567", BankName = "\u0645\u0644\u062A", Branch = "\u062A\u0647\u0631\u0627\u0646", Amount = 2000000, DueDate = DateTime.Now.AddDays(15), PayerName = "\u0639\u0644\u06CC", ReceiverName = "\u0641\u0631\u0648\u0634\u06AF\u0627\u0647", Type = ChequeType.Receivable, Status = ChequeStatus.InVault },
            new Cheque { ChequeNumber = "9876543", BankName = "\u0635\u0627\u062F\u0631\u0627\u062A", Branch = "\u0634\u06CC\u0631\u0627\u0632", Amount = 1500000, DueDate = DateTime.Now.AddDays(30), PayerName = "\u0641\u0631\u0648\u0634\u06AF\u0627\u0647", ReceiverName = "\u06AF\u0631\u0648\u0647", Type = ChequeType.Payable, Status = ChequeStatus.InVault },
            new Cheque { ChequeNumber = "5555555", BankName = "\u067E\u0627\u0633\u0627\u0631\u06AF\u0627\u062F", Branch = "\u0627\u0635\u0641\u0647\u0627\u0646", Amount = 3000000, DueDate = DateTime.Now.AddDays(-5), PayerName = "\u0645\u062D\u0645\u062F", ReceiverName = "\u0641\u0631\u0648\u0634\u06AF\u0627\u0647", Type = ChequeType.Receivable, Status = ChequeStatus.Passed }
        );

        // Expenses
        db.Expenses.AddRange(
            new Expense { Category = ExpenseCategory.Rent, Description = "\u0627\u062C\u0627\u0631\u0647 \u0645\u0627\u0647\u0627\u0646\u0647", Amount = 15000000, Date = DateTime.Now.AddDays(-10) },
            new Expense { Category = ExpenseCategory.Utilities, Description = "\u0642\u0628\u0636 \u0628\u0631\u0642", Amount = 2500000, Date = DateTime.Now.AddDays(-15) },
            new Expense { Category = ExpenseCategory.Salary, Description = "\u062D\u0642\u0648\u0642 \u06A9\u0627\u0631\u0645\u0646\u062F", Amount = 25000000, Date = DateTime.Now.AddDays(-5) },
            new Expense { Category = ExpenseCategory.Transport, Description = "\u0647\u0632\u06CC\u0646\u0647 \u062D\u0645\u0644 \u0628\u0627\u0631", Amount = 800000, Date = DateTime.Now.AddDays(-3) },
            new Expense { Category = ExpenseCategory.Maintenance, Description = "\u062A\u0639\u0645\u06CC\u0631\u0627\u062A \u0645\u06AF\u0627\u0632\u0647", Amount = 1200000, Date = DateTime.Now.AddDays(-2) }
        );

        // Suppliers
        db.Suppliers.AddRange(
            new Supplier { Name = "\u067E\u062E\u0634 \u0633\u0631\u0627\u0633\u0631\u06CC", Phone = "02188776655", Address = "\u062A\u0647\u0631\u0627\u0646", ContactPerson = "\u0622\u0642\u0627\u06CC \u0627\u062D\u0645\u062F\u06CC", TotalDebt = 45000000, TotalPaid = 30000000 },
            new Supplier { Name = "\u0644\u0628\u0646\u06CC\u0627\u062A \u0634\u0645\u0627\u0644", Phone = "01133445566", Address = "\u0633\u0627\u0631\u06CC", ContactPerson = "\u062E\u0627\u0646\u0645 \u0631\u0633\u062A\u0645\u06CC", TotalDebt = 12000000, TotalPaid = 12000000 },
            new Supplier { Name = "\u062E\u0634\u06A9\u0628\u0627\u0631 \u0627\u0635\u0641\u0647\u0627\u0646", Phone = "03112345678", Address = "\u0627\u0635\u0641\u0647\u0627\u0646", ContactPerson = "\u0622\u0642\u0627\u06CC \u0646\u0648\u0631\u06CC", TotalDebt = 8000000, TotalPaid = 5000000 }
        );

        db.SaveChanges();
    }
}
