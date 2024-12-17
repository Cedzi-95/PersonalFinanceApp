using System.Globalization;
using Npgsql;

public class PostgresAccount : IaccountManager
{
    private IUserService userService;
    private NpgsqlConnection connection;
    public decimal balance { get; set; }
    private List<Transaction> transactions = new List<Transaction>();

    public PostgresAccount(IUserService userService, NpgsqlConnection connection, decimal balance)
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


        var accountId = GetAccountIdForUser(user.UserId);
        if (accountId == null)
        {
            throw new Exception($"{Colours.RED}No account found for this user{Colours.NORMAL}");
        }

        var transaction = CollectTransactionDetails();

        ProcessTransaction(transaction);
        SaveTransactionToDatabase(transaction, accountId.Value, user.UserId);
        DisplayTransactionSummary(transaction);
        transactions.Add(transaction);

    }


    private Guid? GetAccountIdForUser(Guid userId)
    {
        const string sql = "SELECT account_id From accounts WHERE user_id = @user_id";
        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@user_id", userId);

        var result = cmd.ExecuteScalar();
        return result != null ? (Guid)result : null;
    }


    // public void CreateAccountForUser(Guid userId)
    // {
    //     const string sql = "INSERT INTO accounts (account_id, user_id, balance) VALUES (@account_id, @user_id, 0)";
    //     using var cmd = new NpgsqlCommand(sql, connection);
    //     cmd.Parameters.AddWithValue("@account_id", Guid.NewGuid());
    //     cmd.Parameters.AddWithValue("@user_id", userId);


    // }
    private Transaction CollectTransactionDetails()
    {
        Console.Clear();
        var TransactionType = getTransactionType();
        var amount = getAmount();
        var date = GetTransactionDate();

        return new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Date = date,
            TransactionType = TransactionType,
            Amount = amount
        };

    }
    private string getTransactionType()
    {
        while (true)
        {
            Console.WriteLine($"Transaction type: Enter {Colours.GREEN}[d] for deposition{Colours.NORMAL} or {Colours.BLUE}[w] for Withdrawal{Colours.NORMAL} ");
            string input = Console.ReadLine()!.ToLower();

            if (input == "d" || input == "w")
            {
                return input;
            }
            else
            {
                System.Console.WriteLine($"{Colours.RED}Invalid transaction type. Please enter 'd' or 'w'.{Colours.NORMAL}");
            }


        }
    }

    private decimal getAmount()
    {
        while (true)
        {
            System.Console.WriteLine("Enter amount: ");
            if (decimal.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount) && amount > 0)
            {
                return amount;
            }
            Console.WriteLine($"{Colours.RED}Please enter a valid positive amount. Use '.' for decimals.{Colours.NORMAL}");
        }
    }

    private DateTime GetTransactionDate()
    {
        while (true)
        {
            Console.WriteLine("Enter date (yyyy-MM-dd): ");
            string input = Console.ReadLine() ?? "";

            if (DateTime.TryParseExact(input, "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            Console.WriteLine($"{Colours.RED}Invalid date format. Please use yyyy-MM-dd format.{Colours.NORMAL}");

        }

    }


    private void ProcessTransaction(Transaction transaction)
    {
        if (transaction.TransactionType == "d")
        {
            balance += transaction.Amount;
            System.Console.WriteLine($"Deposition amount : {Colours.GREEN}{transaction.Amount}{Colours.NORMAL} | New balance : {Colours.GREEN}{balance}{Colours.NORMAL}");
        }
        else if (transaction.TransactionType == "w")
        {
            if (transaction.Amount > balance)
            {
                throw new ArgumentException($"{Colours.RED}Insufficient funds. Available balance: {balance}{Colours.NORMAL}");

            }
            balance -= transaction.Amount;
            System.Console.WriteLine($"Withdrawn amount:{Colours.RED}{transaction.Amount}{Colours.NORMAL} | New balance : {Colours.RED}{balance}{Colours.NORMAL}");
        }
    }

    private void SaveTransactionToDatabase(Transaction transaction, Guid accountId, Guid userId)
    {
        const string sql = @"
        INSERT INTO transactions (transaction_id, account_id, date, amount, type) 
        VALUES (@transaction_id, @account_id, @date, @amount, @type)";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@transaction_id", transaction.TransactionId);
        cmd.Parameters.AddWithValue("@account_id", accountId);
        cmd.Parameters.AddWithValue("@date", transaction.Date);
        cmd.Parameters.AddWithValue("@amount", transaction.Amount);
        cmd.Parameters.AddWithValue("@type", transaction.TransactionType);



        var accountSql = @"INSERT INTO accounts (account_id, user_id, balance) VALUES (@account_id, @user_id, @balance)";
        using var accountCmd = new NpgsqlCommand(accountSql, connection);
        accountCmd.Parameters.AddWithValue("@account_id", accountId);
         accountCmd.Parameters.AddWithValue("@user_id", userId);
          accountCmd.Parameters.AddWithValue("@balance", balance += transaction.Amount);


        cmd.ExecuteNonQuery();
    }


    private void DisplayTransactionSummary(Transaction transaction)
    {
        Console.WriteLine("\nTransaction Summary:");
        Console.WriteLine(transaction.ToString());
        Console.WriteLine($"Current Balance: {Colours.GREEN}{balance}{Colours.NORMAL}");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    public void DeleteTransactions()
    {
        throw new NotImplementedException();
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

    Transaction IaccountManager.CollectTransactionDetails()
    {
        throw new NotImplementedException();
    }

    // public void PrintAllTransactions(Transaction transaction, Guid accountId)
    // {
    //     var sql = @"SELECT FROM transactions WHERE account_id = @account_id";
    //     using var cmd = new NpgsqlCommand(sql, connection);
    //     cmd.Parameters.AddWithValue("@account_id", accountId);
    // }

    public List<Transaction> PrintAllTransactions(Guid accountId)
    {
        
       var sql = @"SELECT u.username, t.type, t.amount, t.date
       FROM users u
       JOIN accounts a on a.user_id = u.user_id
       JOIN transactions t on t.account_id = a.account_id
       WHERE account_id = @accountId  ";
        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@account_id", accountId);

        using var reader = cmd.ExecuteReader();

        List<Transaction> transactions = new List<Transaction>();
        while (reader.Read())
        {
            Transaction transaction = new Transaction
            {
                TransactionId = reader.GetGuid(0),
                User = reader.IsDBNull(1) ? null : new User
                {
                    UserId = reader.GetGuid(1),
                    Name = reader.GetString(2),
                    Password = ""

                },
                Date = reader.GetDateTime(3),
                TransactionType=reader.GetString(4),
                Amount = reader.GetDecimal(5)

            };
            transactions.Add(transaction);
        }
        return transactions;

    }
}






// public void Deposition(decimal amount)
// {
//     Console.Clear();
//     balance += amount;
//     System.Console.WriteLine($" Deposition amount: {Colours.GREEN} {amount} {Colours.NORMAL} \n New balance:{Colours.GREEN} {balance} {Colours.NORMAL}");
//     System.Console.WriteLine("\n Press key to continue...");
//     Console.ReadKey();
// }



// public void Withdraw(decimal amount)
// {
//     Console.Clear();
//     if (amount <= balance)
//     {

//         balance -= amount;
//         System.Console.WriteLine($"> Amount retreived:{Colours.RED} {amount} {Colours.NORMAL} \n{Colours.GREEN}New balance: {balance}");
//         System.Console.WriteLine($"{Colours.NORMAL}");
//         System.Console.WriteLine("\n Press key to continue...");
//         Console.ReadKey();

//     }
//     else
//     {
//         System.Console.WriteLine($"{Colours.RED}Insufficient funds {Colours.NORMAL}");
//         Console.ReadKey();
//     }
