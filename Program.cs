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
        
        // Keep the console window open
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
