// Transaction record
public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

// Transaction Processor Interface
public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

// Concrete processor implementations
public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Bank Transfer] Processing ${transaction.Amount} for {transaction.Category}");
    }
}

public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Mobile Money] Processing ${transaction.Amount} for {transaction.Category}");
    }
}

public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Crypto Wallet] Processing ${transaction.Amount} for {transaction.Category}");
    }
}

// Base Account class
public class Account
{
    public string AccountNumber { get; }
    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
        Console.WriteLine($"Transaction applied. New balance: ${Balance}");
    }
}

// Sealed SavingsAccount class
public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance) 
        : base(accountNumber, initialBalance) {}

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds");
            return;
        }
        
        base.ApplyTransaction(transaction);
    }
}

// Main application class
public class FinanceApp
{
    private readonly List<Transaction> _transactions = new();

    public void Run()
    {
        // Create a savings account with initial balance of 1000
        var savingsAccount = new SavingsAccount("SAV123456789", 1000m);
        Console.WriteLine($"Account {savingsAccount.AccountNumber} created with initial balance: ${savingsAccount.Balance}");

        // Create sample transactions
        var transactions = new List<Transaction>
        {
            new(1, DateTime.Now, 150m, "Groceries"),
            new(2, DateTime.Now, 75.50m, "Utilities"),
            new(3, DateTime.Now, 45.99m, "Entertainment")
        };

        // Create processors
        var mobileMoneyProcessor = new MobileMoneyProcessor();
        var bankTransferProcessor = new BankTransferProcessor();
        var cryptoWalletProcessor = new CryptoWalletProcessor();

        // Process transactions
        Console.WriteLine("\nProcessing transactions...");
        
        // Transaction 1 - Mobile Money
        var transaction1 = transactions[0];
        mobileMoneyProcessor.Process(transaction1);
        savingsAccount.ApplyTransaction(transaction1);
        _transactions.Add(transaction1);

        // Transaction 2 - Bank Transfer
        var transaction2 = transactions[1];
        bankTransferProcessor.Process(transaction2);
        savingsAccount.ApplyTransaction(transaction2);
        _transactions.Add(transaction2);

        // Transaction 3 - Crypto Wallet
        var transaction3 = transactions[2];
        cryptoWalletProcessor.Process(transaction3);
        savingsAccount.ApplyTransaction(transaction3);
        _transactions.Add(transaction3);

        Console.WriteLine($"\nTotal transactions processed: {_transactions.Count}");
        Console.WriteLine($"Final account balance: ${savingsAccount.Balance}");
    }
}

// ============================================
// Healthcare System Implementation
// ============================================

// Generic Repository for entity management
public class Repository<T> where T : class
{
    private readonly List<T> _items = new();

    public void Add(T item)
    {
        _items.Add(item);
    }

    public List<T> GetAll()
    {
        return new List<T>(_items);
    }

    public T? GetById(Func<T, bool> predicate)
    {
        return _items.FirstOrDefault(predicate);
    }

    public bool Remove(Func<T, bool> predicate)
    {
        var item = _items.FirstOrDefault(predicate);
        if (item != null)
        {
            return _items.Remove(item);
        }
        return false;
    }
}

// Patient class
public class Patient
{
    public int Id { get; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }

    public Patient(int id, string name, int age, string gender)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Age = age;
        Gender = gender ?? throw new ArgumentNullException(nameof(gender));
    }

    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Age: {Age}, Gender: {Gender}";
    }
}

// Prescription class
public class Prescription
{
    public int Id { get; }
    public int PatientId { get; }
    public string MedicationName { get; set; }
    public DateTime DateIssued { get; set; }

    public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
    {
        Id = id;
        PatientId = patientId;
        MedicationName = medicationName ?? throw new ArgumentNullException(nameof(medicationName));
        DateIssued = dateIssued;
    }

    public override string ToString()
    {
        return $"Prescription ID: {Id}, Medication: {MedicationName}, Date Issued: {DateIssued:yyyy-MM-dd}";
    }
}

// Health System Application
public class HealthSystemApp
{
    private readonly Repository<Patient> _patientRepo = new();
    private readonly Repository<Prescription> _prescriptionRepo = new();
    private readonly Dictionary<int, List<Prescription>> _prescriptionMap = new();

    public void SeedData()
    {
        // Add sample patients
        _patientRepo.Add(new Patient(1, "Dickson Daniel Peprah", 35, "Male"));
        _patientRepo.Add(new Patient(2, "Jane Smith", 28, "Female"));
        _patientRepo.Add(new Patient(3, "Robert Johnson", 42, "Male"));

        // Add sample prescriptions
        _prescriptionRepo.Add(new Prescription(101, 1, "Ibuprofen", DateTime.Now.AddDays(-10)));
        _prescriptionRepo.Add(new Prescription(102, 1, "Amoxicillin", DateTime.Now.AddDays(-5)));
        _prescriptionRepo.Add(new Prescription(103, 2, "Loratadine", DateTime.Now.AddDays(-3)));
        _prescriptionRepo.Add(new Prescription(104, 3, "Metformin", DateTime.Now.AddDays(-1)));
        _prescriptionRepo.Add(new Prescription(105, 2, "Lisinopril", DateTime.Now));
    }

    public void BuildPrescriptionMap()
    {
        _prescriptionMap.Clear();
        
        // Group prescriptions by PatientId
        var allPrescriptions = _prescriptionRepo.GetAll();
        foreach (var prescription in allPrescriptions)
        {
            if (!_prescriptionMap.ContainsKey(prescription.PatientId))
            {
                _prescriptionMap[prescription.PatientId] = new List<Prescription>();
            }
            _prescriptionMap[prescription.PatientId].Add(prescription);
        }
    }

    public void PrintAllPatients()
    {
        Console.WriteLine("\n=== All Patients ===");
        var patients = _patientRepo.GetAll();
        foreach (var patient in patients)
        {
            Console.WriteLine(patient);
        }
    }

    public void PrintPrescriptionsForPatient(int patientId)
    {
        var patient = _patientRepo.GetById(p => p.Id == patientId);
        if (patient == null)
        {
            Console.WriteLine($"\nPatient with ID {patientId} not found.");
            return;
        }

        Console.WriteLine($"\n=== Prescriptions for {patient.Name} (ID: {patient.Id}) ===");
        
        if (_prescriptionMap.TryGetValue(patientId, out var prescriptions) && prescriptions.Any())
        {
            foreach (var prescription in prescriptions)
            {
                Console.WriteLine($"- {prescription}");
            }
        }
        else
        {
            Console.WriteLine("No prescriptions found for this patient.");
        }
    }

    public List<Prescription> GetPrescriptionsByPatientId(int patientId)
    {
        return _prescriptionMap.TryGetValue(patientId, out var prescriptions) 
            ? prescriptions 
            : new List<Prescription>();
    }
}

// ============================================
// Warehouse Inventory Management System
// ============================================

// Marker Interface for Inventory Items
public interface IInventoryItem
{
    int Id { get; }
    string Name { get; }
    int Quantity { get; set; }
}

// Custom Exceptions
public class DuplicateItemException : Exception
{
    public DuplicateItemException(string message) : base(message) { }
}

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string message) : base(message) { }
}

public class InvalidQuantityException : Exception
{
    public InvalidQuantityException(string message) : base(message) { }
}

// Product Classes
public class ElectronicItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public string Brand { get; }
    public int WarrantyMonths { get; }

    public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Quantity = quantity >= 0 ? quantity : throw new ArgumentOutOfRangeException(nameof(quantity));
        Brand = brand ?? throw new ArgumentNullException(nameof(brand));
        WarrantyMonths = warrantyMonths >= 0 ? warrantyMonths : throw new ArgumentOutOfRangeException(nameof(warrantyMonths));
    }

    public override string ToString()
    {
        return $"Electronic: {Name} (ID: {Id}), Brand: {Brand}, Quantity: {Quantity}, Warranty: {WarrantyMonths} months";
    }
}

public class GroceryItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; }

    public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Quantity = quantity >= 0 ? quantity : throw new ArgumentOutOfRangeException(nameof(quantity));
        ExpiryDate = expiryDate;
    }

    public override string ToString()
    {
        return $"Grocery: {Name} (ID: {Id}), Quantity: {Quantity}, Expires: {ExpiryDate:yyyy-MM-dd}";
    }
}

// Generic Inventory Repository
public class InventoryRepository<T> where T : IInventoryItem
{
    private readonly Dictionary<int, T> _items = new();

    public void AddItem(T item)
    {
        if (_items.ContainsKey(item.Id))
        {
            throw new DuplicateItemException($"Item with ID {item.Id} already exists.");
        }
        _items[item.Id] = item;
    }

    public T GetItemById(int id)
    {
        if (!_items.TryGetValue(id, out var item))
        {
            throw new ItemNotFoundException($"Item with ID {id} not found.");
        }
        return item;
    }

    public void RemoveItem(int id)
    {
        if (!_items.ContainsKey(id))
        {
            throw new ItemNotFoundException($"Cannot remove. Item with ID {id} not found.");
        }
        _items.Remove(id);
    }

    public List<T> GetAllItems()
    {
        return _items.Values.ToList();
    }

    public void UpdateQuantity(int id, int newQuantity)
    {
        if (newQuantity < 0)
        {
            throw new InvalidQuantityException($"Quantity cannot be negative. Attempted to set {newQuantity} for item ID {id}.");
        }

        if (!_items.TryGetValue(id, out var item))
        {
            throw new ItemNotFoundException($"Cannot update. Item with ID {id} not found.");
        }

        item.Quantity = newQuantity;
    }
}

// Warehouse Manager
public class WareHouseManager
{
    private readonly InventoryRepository<ElectronicItem> _electronics = new();
    private readonly InventoryRepository<GroceryItem> _groceries = new();

    public void SeedData()
    {
        // Add sample electronic items
        _electronics.AddItem(new ElectronicItem(1, "Smartphone", 50, "Samsung", 24));
        _electronics.AddItem(new ElectronicItem(2, "Laptop", 30, "Dell", 36));
        _electronics.AddItem(new ElectronicItem(3, "Headphones", 100, "Sony", 12));

        // Add sample grocery items
        _groceries.AddItem(new GroceryItem(101, "Milk", 200, DateTime.Now.AddDays(7)));
        _groceries.AddItem(new GroceryItem(102, "Bread", 150, DateTime.Now.AddDays(3)));
        _groceries.AddItem(new GroceryItem(103, "Eggs", 300, DateTime.Now.AddDays(21)));
    }

    public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
    {
        var items = repo.GetAllItems();
        Console.WriteLine($"\n=== {typeof(T).Name}s ===");
        if (items.Count == 0)
        {
            Console.WriteLine("No items found.");
            return;
        }

        foreach (var item in items)
        {
            Console.WriteLine(item);
        }
    }

    public void IncreaseStock<T>(InventoryRepository<T> repo, int id, int quantity) where T : IInventoryItem
    {
        try
        {
            var item = repo.GetItemById(id);
            repo.UpdateQuantity(id, item.Quantity + quantity);
            Console.WriteLine($"Increased stock for item ID {id} by {quantity}. New quantity: {item.Quantity + quantity}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error increasing stock: {ex.Message}");
        }
    }

    public void RemoveItemById<T>(InventoryRepository<T> repo, int id) where T : IInventoryItem
    {
        try
        {
            repo.RemoveItem(id);
            Console.WriteLine($"Item with ID {id} has been removed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing item: {ex.Message}");
        }
    }

    // Helper methods to access repositories
    public InventoryRepository<ElectronicItem> Electronics => _electronics;
    public InventoryRepository<GroceryItem> Groceries => _groceries;
}

// Entry point
class Program
{
    static void Main(string[] args)
    {
        // Run Finance Application
        var financeApp = new FinanceApp();
        financeApp.Run();
        
        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("HEALTHCARE SYSTEM");
        Console.WriteLine(new string('=', 50));
        
        // Run Healthcare Application
        var healthApp = new HealthSystemApp();
        
        // Seed data
        healthApp.SeedData();
        
        // Build prescription map
        healthApp.BuildPrescriptionMap();
        
        // Display all patients
        healthApp.PrintAllPatients();
        
        // Display prescriptions for a specific patient (using first patient's ID)
        healthApp.PrintPrescriptionsForPatient(1);
        
        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("WAREHOUSE INVENTORY SYSTEM");
        Console.WriteLine(new string('=', 50));
        
        // Run Warehouse Application
        RunWarehouseDemo();
        
        // Keep the console window open
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static void RunWarehouseDemo()
    {
        var warehouse = new WareHouseManager();
        
        try
        {
            // Seed initial data
            warehouse.SeedData();
            
            // Print all items
            warehouse.PrintAllItems(warehouse.Groceries);
            warehouse.PrintAllItems(warehouse.Electronics);
            
            // Demonstrate error handling
            Console.WriteLine("\n=== Testing Error Handling ===");
            
            // Try to add a duplicate item
            try
            {
                warehouse.Electronics.AddItem(new ElectronicItem(1, "Duplicate Phone", 10, "Test", 12));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Expected error (adding duplicate): {ex.Message}");
            }
            
            // Try to remove non-existent item
            try
            {
                warehouse.RemoveItemById(warehouse.Groceries, 999);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Expected error (removing non-existent): {ex.Message}");
            }
            
            // Try to update with invalid quantity
            try
            {
                warehouse.Electronics.UpdateQuantity(1, -5);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Expected error (invalid quantity): {ex.Message}");
            }
            
            // Demonstrate successful operations
            Console.WriteLine("\n=== Demonstrating Successful Operations ===");
            warehouse.IncreaseStock(warehouse.Groceries, 101, 50); // Add 50 to Milk
            warehouse.RemoveItemById(warehouse.Electronics, 3); // Remove Headphones
            
            // Print final state
            Console.WriteLine("\n=== Final Inventory State ===");
            warehouse.PrintAllItems(warehouse.Groceries);
            warehouse.PrintAllItems(warehouse.Electronics);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
}
