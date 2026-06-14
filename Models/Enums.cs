namespace PosAccountingApp.Models;

public enum UserRole { SuperAdmin, Admin, Cashier, Broker, Accountant }

public enum UnitType { Number, Weight, Box, Meter, Ton, Bag, CubicMeter }

public enum PropertyType { Apartment, Villa, Commercial, Land }

public enum DealType { Sale, Rent, Mortgage }

public enum PropertyStatus { Available, Archived, Dealt }

public enum VehicleStatus { InShowroom, Sold, Returned, Reserved }

public enum ChequeType { Receivable, Payable }

public enum ChequeStatus { InVault, Passed, Returned, Transferred }

public enum PaymentMethod { Cash, Card, Ledger, Installments, Mixed }

public enum SaleStatus { Normal, Returned }

public enum LedgerType { Charge, Payment, Sale }

public enum ExpenseCategory { Rent, Utilities, Salary, Supplies, Maintenance, Transport, Other }

public enum PaymentStatus { Unpaid, PartiallyPaid, FullyPaid, Overdue }

public enum BusinessProfile { Supermarket, Boutique, RealEstate, CarDealership, ConstructionMaterials }

public enum RoundingMode { Off, FiveHundred, Thousand }
