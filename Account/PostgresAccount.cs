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
         this.balance = balance;
    }

    public void CheckBalance()
    {
       System.Console.WriteLine($"\n{Colours.GREEN} Current balance:{Colours.GREEN} {balance}");
        System.Console.WriteLine($"{Colours.NORMAL}");
        System.Console.WriteLine("\n press key to continiue..");
        Console.ReadKey();
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
         Console.Clear();
        balance += amount;
        System.Console.WriteLine($" Deposition amount: {Colours.GREEN} {amount} {Colours.NORMAL} \n New balance:{Colours.GREEN} {balance} {Colours.NORMAL}");
        System.Console.WriteLine("\n Press key to continue...");
        Console.ReadKey();
    }

    public void PrintAllTransactions()
    {
           System.Console.WriteLine($"{Colours.GREEN}List of all your transactions {Colours.NORMAL}");
        System.Console.WriteLine();
        foreach (Transaction transaction in transactions)
        {

            System.Console.WriteLine(transaction.ToString());
            System.Console.WriteLine();
        }


        Console.ReadKey();

    }

    public void PrintExpenditures()
    {
        Expenditure expenditure = new Expenditure(transactions); //new instance of expediture object

        try
        {
            while (true)
            {
                Console.WriteLine("Type one of the alternatives below to view spending stats :\n 'year' 'month' 'week' 'day' 'exit' ");
                string myChoice = Console.ReadLine()!.ToLower();

                switch (myChoice)
                {
                    case "year":
                        expenditure.AnnualSpending();
                        break;
                    case "month":
                        expenditure.MonthSpend();
                        break;
                    case "week":
                        expenditure.WeeklyExpenditure();
                        break;
                    case "day":
                        expenditure.DailySpending();
                        break;
                    case "exit":
                        Console.WriteLine("back to the meny...");
                        return;

                }
            }
        }
        catch
        {
            throw new ArgumentNullException($"{Colours.RED}Invalid input {Colours.NORMAL}");
        }
    }

    public void PrintIncome()
    {
       Income income = new Income(transactions); //new instance of an income object
        try
        {
            while (true)
            {
                Console.WriteLine($"Type one of the alternatives below to view income stats :\n'year' 'month' 'week' 'day' 'exit' ");
                string myChoice = Console.ReadLine()!.ToLower();

                switch (myChoice)
                {
                    case "year":
                        income.YearIncome();
                        break;
                    case "month":
                        income.MonthlyIncome();
                        break;
                    case "week":
                        income.WeekIncome();
                        break;
                    case "day":
                        income.DailyIncome();
                        break;

                    case "exit":
                        System.Console.WriteLine("Back to the main menu.");
                        return;
                }

            }
        }
        catch
        {
            throw new ArgumentNullException($"{Colours.RED}Invalid input! {Colours.NORMAL}");
        }


    }

    public void Withdraw(decimal amount)
    {
        Console.Clear();
        if (amount <= balance)
        {
           
            balance -= amount;
            System.Console.WriteLine($"> Amount retreived:{Colours.RED} {amount} {Colours.NORMAL} \n{Colours.GREEN}New balance: {balance}");
            System.Console.WriteLine($"{Colours.NORMAL}");
            System.Console.WriteLine("\n Press key to continue...");
            Console.ReadKey();

        }
        else
        {
             System.Console.WriteLine($"{Colours.RED}Insufficient funds {Colours.NORMAL}");
           Console.ReadKey();
        }
    }
}