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

// Entry point
class Program
{
    static void Main(string[] args)
    {
        var app = new FinanceApp();
        app.Run();
        
        // Keep the console window open
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
