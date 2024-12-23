using System.Data;
using System.Globalization;
using Npgsql;

public class PostgresAccount : IaccountManager
{
    private IUserService userService;
    private NpgsqlConnection connection;
    private List<Transaction> transactions = new List<Transaction>();
    private UserMenu userMenu;

    public PostgresAccount(IUserService userService, NpgsqlConnection connection)
    {
        this.userService = userService;
        this.connection = connection;
    }

    public decimal CheckBalance()
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

        var sql = @"
        SELECT COALESCE(SUM(amount), 0) AS balance
        FROM transactions
        WHERE account_id = @accountId;";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@accountId", accountId);

        using var reader = cmd.ExecuteReader();

        reader.Read();

        decimal balance = reader.GetDecimal(0);

        return balance;

    }



    public List<Transaction> PrintAllTransactions(Guid UserId)
    {
        var sql = @"SELECT  t.type, t.amount, t.date, t.transaction_id
       FROM users u
       JOIN accounts a on a.user_id = u.user_id
       JOIN transactions t on t.account_id = a.account_id
       WHERE u.user_id = @UserId  ";
        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@UserId", UserId);

        using var reader = cmd.ExecuteReader();
        List<Transaction> transactions = new List<Transaction>();
        while (reader.Read())
        {
            Transaction transaction = new Transaction
            {
                TransactionId = reader.GetGuid(3),
                User = null,
                Date = reader.GetDateTime(2),
                TransactionType = reader.GetString(0),
                Amount = reader.GetDecimal(1)

            };
            transactions.Add(transaction);
        }
        return transactions;

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

        // ProcessTransaction(transaction);
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


    private void SaveTransactionToDatabase(Transaction transaction, Guid accountId, Guid userId)
    {
        if (transaction.TransactionType == "d")
        {
            const string sql = @"
        BEGIN TRANSACTION;

        INSERT INTO transactions (transaction_id, account_id, date, amount, type) 
        VALUES (@transaction_id, @accountId, @date, @amount, @type);
        commit;
        
       ";

            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@transaction_id", transaction.TransactionId);
            cmd.Parameters.AddWithValue("@accountId", accountId);
            cmd.Parameters.AddWithValue("@date", transaction.Date);
            cmd.Parameters.AddWithValue("@amount", transaction.Amount);
            cmd.Parameters.AddWithValue("@type", transaction.TransactionType);
            cmd.ExecuteNonQuery();
        }

        else if (transaction.TransactionType == "w")
        {
const string sql = @"
WITH locked_rows AS (
    SELECT amount
    FROM transactions 
    WHERE account_id = @account_id
    FOR UPDATE
),
balance AS (
    SELECT COALESCE(SUM(amount), 0) as total_amount
    FROM locked_rows
)
INSERT INTO transactions (
    transaction_id, 
    account_id, 
    date, 
    amount, 
    type
)
SELECT 
    @transaction_id,
    @account_id,
    @date,
    -@amount,
    @type
WHERE (SELECT total_amount FROM balance) >= @amount;";

using var cmd = new NpgsqlCommand(sql, connection);
cmd.Parameters.AddWithValue("@transaction_id", transaction.TransactionId);
cmd.Parameters.AddWithValue("@account_id", accountId);
cmd.Parameters.AddWithValue("@date", transaction.Date);
cmd.Parameters.AddWithValue("@amount", transaction.Amount);
cmd.Parameters.AddWithValue("@type", transaction.TransactionType);

int rowsAffected = cmd.ExecuteNonQuery();
if (rowsAffected == 0)
{
    throw new Exception($"{Colours.RED}Insufficient funds{Colours.NORMAL}");
}


        }
    }


    private void DisplayTransactionSummary(Transaction transaction)
    {
        Console.WriteLine("\nTransaction Summary:");
        System.Console.WriteLine($"{Colours.ORANGE}");
        Console.WriteLine(transaction.ToString());
        Console.WriteLine($"> New balance:{Colours.GREEN} {CheckBalance():c}");
        Console.WriteLine($"{Colours.NORMAL}");
    }

    public async Task DeleteTransactions()
    {
        try
        {
 var user = userService.GetLoggedInUser();
     var accountId = GetAccountIdForUser(user.UserId);


     System.Console.WriteLine("Delete [all] or [single] transactions");
      string input = Console.ReadLine()!.ToLower();

      

      if(input == "all")
      {
        var sql = @"DELETE FROM transactions WHERE account_id = @accountId::uuid
        ";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@accountId", accountId);

        var rowsAffected = await cmd.ExecuteNonQueryAsync();
        System.Console.WriteLine($"{rowsAffected} transactions were deleted");

      }

      else if (input == "single")
      {
         DateTime date = GetTransactionDate();

         var sql = @"DELETE FROM transactions WHERE date = @date
         AND account_id = @accountId";

         using var cmd = new NpgsqlCommand(sql, connection);
         cmd.Parameters.AddWithValue("@date", date);
         cmd.Parameters.AddWithValue("@accountId", accountId);

         var rowsAffected = await cmd.ExecuteNonQueryAsync();
         System.Console.WriteLine($"{rowsAffected} transaction(s) were deleted");

      }

      else
      {
        Console.WriteLine("Invalid input. Please enter 'all' or 'single'.");
      }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error deleting transactions: {ex.Message}");
            throw;
        }
    


    }


    public void PrintExpenditures()
    {
       

        Expenditure expenditure = new Expenditure(connection, userService);

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
                        Console.WriteLine("back to the main menu...");                        
                        
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
        
        Income income = new Income( connection, userService); 
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
                        
                        System.Console.WriteLine("Back to the menu");
                        
                        break;
                        
                }

            }
        }
        catch
        {
            throw new ArgumentNullException($"{Colours.RED}Invalid input! {Colours.NORMAL}");
        }


    }



}






