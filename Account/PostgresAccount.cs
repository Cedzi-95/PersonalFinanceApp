using System.Globalization;
using Npgsql;

public class PostgresAccount : IaccountManager
{
    private IUserService userService;
    private NpgsqlConnection connection;
    public decimal balance {get; set;}
     private List<Transaction> transactions = new List<Transaction>();

    public PostgresAccount(IUserService userService, NpgsqlConnection connection)
    {
        this.userService = userService;
        this.connection = connection;
        // this.balance = balance;
    }

    public void CheckBalance()
    {
        throw new NotImplementedException();
    }

    public void CreateTransaction()
    {
       
        var user = userService.GetLoggedInUser();
        if (user == null)
        {
            throw new ArgumentException("You are not logged in.");
        }

        var account = new Account
        {
            AccountId = new Guid(),
            Balance = balance,
            
        };


       
          Console.Clear();
        Console.WriteLine($"Transaction type: Enter {Colours.GREEN}[d] for deposition{Colours.NORMAL} or {Colours.BLUE}[w] for Withdrawal{Colours.NORMAL} ");
        string transactionInput = Console.ReadLine()!;

        System.Console.WriteLine("Enter amount: ");
        decimal amount;

        if (!decimal.TryParse(Console.ReadLine()!, out amount) || amount <= 0)
        {
            System.Console.WriteLine($"{Colours.RED}");
            System.Console.WriteLine($"Usage: Enter positive amount and use a ',' if amount is a decimal!{Colours.NORMAL}");
            Console.ReadKey();
            return;
        }





        System.Console.WriteLine("Enter date: (yyyy-MM-dd) ");
        string dateInput = Console.ReadLine()!;
        DateTime date;
        try
        {
            date = DateTime.ParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
           
            System.Console.WriteLine($"{Colours.RED}Invalid date input, please enter the date in the correct format{Colours.NORMAL}");
            Console.ReadKey();
            throw new ArgumentException();
           

        }

        Transaction transaction = new Transaction
        {
            TransactionId = new Guid(),
            Date = date,
            TransactionType = transactionInput,
            Amount = amount,
        };


        if (transactionInput.Equals("d"))
        {
            Deposition(amount);
        }
        else if (transactionInput.Equals("w") && amount <= balance)
        {

            Withdraw(amount);
            
        }
        
        else 
        {
            System.Console.WriteLine($"{Colours.RED}Invalid transaction type or insuffiecient funds{Colours.NORMAL}");
            Console.ReadKey();
             return;
        }

        var sql = @"Insert into transactions (transaction_id, account_id, data, amount, type) values
        (@transaction_id, @account_id, @date, @amount, @type );";
        using var cmd = new NpgsqlCommand(sql, this.connection);
        cmd.Parameters.AddWithValue("@transaction_id", transaction.TransactionId);
        cmd.Parameters.AddWithValue("@account_id", account.AccountId);
        cmd.Parameters.AddWithValue("@date", transaction.Date);
        cmd.Parameters.AddWithValue("@amount", transaction.Amount);
        cmd.Parameters.AddWithValue("@type", transaction.TransactionType);

        transactions.Add(transaction);
        System.Console.WriteLine();
        System.Console.WriteLine(transaction.ToString());
    }

    public void DeleteTransactions()
    {
        throw new NotImplementedException();
    }

    public void Deposition(decimal amount)
    {
        throw new NotImplementedException();
    }

    public void PrintAllTransactions()
    {
        throw new NotImplementedException();
    }

    public void PrintExpenditures()
    {
        throw new NotImplementedException();
    }

    public void PrintIncome()
    {
        throw new NotImplementedException();
    }

    public void Withdraw(decimal amount)
    {
        throw new NotImplementedException();
    }
}