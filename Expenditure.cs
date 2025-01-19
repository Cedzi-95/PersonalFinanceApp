using System.Globalization;
using Npgsql;

public class Expenditure
{
    private NpgsqlConnection connection;
    private IUserService userService;


    public Expenditure(NpgsqlConnection connection, IUserService userService)
    {
        this.connection = connection;
        this.userService = userService;
    }

    private Guid? GetAccountIdForUser(Guid userId)
    {
        const string sql = "SELECT account_id From accounts WHERE user_id = @user_id";
        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@user_id", userId);

        var result = cmd.ExecuteScalar();
        return result != null ? (Guid)result : null;
    }
    public void AnnualSpending()
    {
        var user = userService.GetLoggedInUser();
        var accountId = GetAccountIdForUser(user.UserId);

        System.Console.Write("Enter the year: ");
        int yearInput;
        if (!int.TryParse(Console.ReadLine()!, out yearInput))
        {
            System.Console.WriteLine($"{Colours.RED} Invalid year input! {Colours.NORMAL}");
            return;
        }

        var sql = @" SELECT account_id,
        SUM(amount) as total_spent
        FROM transactions
        WHERE type = 'w'
        AND EXTRACT(YEAR FROM date) = @year
        AND account_id = @accountId::uuid
        GROUP BY account_id

        ";
        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@year", yearInput);
        cmd.Parameters.AddWithValue("@accountId", accountId);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            System.Console.WriteLine("1");
            var yearlySpending = reader.GetDecimal(1);
            System.Console.WriteLine($"Total spending in year {yearInput} : {Colours.RED}{yearlySpending * -1:c}{Colours.NORMAL} ");
            Console.ReadKey();
            System.Console.WriteLine();

        }




    }




    private string GetMonth(int month)
    {
        if (month < 1 || month > 12)
        {
            throw new ArgumentOutOfRangeException("Invalid month input");
        }
        DateTime date = new DateTime(2020, month, 1);
        return date.ToString("MMMM");
    }





    public void MonthSpend()
    {

        Console.Clear();

        var user = userService.GetLoggedInUser();
        var accountId = GetAccountIdForUser(user.UserId);



        System.Console.Write("Enter the month (1-12): ");
        int monthInput;
        if (!int.TryParse(Console.ReadLine()!, out monthInput) || monthInput < 1 || monthInput > 12)
        {
            System.Console.WriteLine("Invalid month input");
            return;
        }


        string monthName = GetMonth(monthInput);



        System.Console.Write("Enter year: ");
        int InputYear;
        if (!int.TryParse(Console.ReadLine()!, out InputYear))
        {
            System.Console.WriteLine("Invalid year input");
            return;
        }

        var sql = @"SELECT account_id,
    SUM(amount) as year_total_income
    FROM transactions
    WHERE type = 'w'
    AND EXTRACT(MONTH FROM date) = @month
    AND EXTRACT(YEAR FROM date) = @year 
    AND account_id = @accountId::uuid  
    GROUP BY account_id";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@month", monthInput);
        cmd.Parameters.AddWithValue("@year", InputYear);
        cmd.Parameters.AddWithValue("@accountId", accountId);



        try
        {
            using var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine($"{Colours.RED}No amount was withdrawn in the given period. {Colours.NORMAL}");
                System.Console.WriteLine();
                return;
            }

            while (reader.Read())
            {
                var totalIncome = reader.GetDecimal(1);
                Console.WriteLine($"Total spending in {monthName} {InputYear} :{Colours.RED} {totalIncome * -1:c}{Colours.NORMAL}");
                System.Console.WriteLine();
                Console.ReadKey();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }



        Console.ReadKey();


    }



    public void WeeklyExpenditure()
    {
        var user = userService.GetLoggedInUser();
        var accountId = GetAccountIdForUser(user.UserId);
        System.Console.Write("Enter week number (1-53): ");
        int weekInput;
        if (!int.TryParse(Console.ReadLine()!, out weekInput) || weekInput < 1 || weekInput > 52)
        {
            System.Console.WriteLine("Invalid week input");
            return;
        }

        System.Console.Write("Enter year: ");
        int yearInput;
        if (!int.TryParse(Console.ReadLine()!, out yearInput))
        {
            System.Console.WriteLine("Invalid year input");
            return;
        }

        var sql = @"SELECT account_id,
    SUM(amount) as year_total_income
    FROM transactions
    WHERE type = 'w'
    AND EXTRACT(WEEK FROM date) = @week
    AND EXTRACT(YEAR FROM date) = @year
    AND account_id = @accountId::uuid  
    GROUP BY account_id";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@week", weekInput);
        cmd.Parameters.AddWithValue("@year", yearInput);
        cmd.Parameters.AddWithValue("@accountId", accountId);



        try
        {
            using var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine($"{Colours.RED}No amount was withdrawn from the given timeline{Colours.NORMAL}");
                return;
            }

            while (reader.Read())
            {
                var totalIncome = reader.GetDecimal(1);
                Console.WriteLine($"Total spending in week {weekInput} of {yearInput}:{Colours.RED} {totalIncome * -1:c}{Colours.NORMAL}");
                System.Console.WriteLine();
                Console.ReadKey();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }


    public void DailySpending()
    {

        Console.Clear();


        System.Console.Write("Enter day of the week (1-7): ");
        int dayInput;
        if (!int.TryParse(Console.ReadLine()!, out dayInput) || dayInput < 0 || dayInput > 7)
        {
            System.Console.WriteLine("Invalid weekday input");
            return;
        }
        if (dayInput == 7)
        {
            dayInput -= 7;
        }

        var user = userService.GetLoggedInUser();
        var accountId = GetAccountIdForUser(user.UserId);
        System.Console.Write("Enter week number (1-53): ");
        int weekInput;
        if (!int.TryParse(Console.ReadLine()!, out weekInput) || weekInput < 1 || weekInput > 52)
        {
            System.Console.WriteLine("Invalid week input");
            return;
        }

        System.Console.Write("Enter year: ");
        int yearInput;
        if (!int.TryParse(Console.ReadLine()!, out yearInput))
        {
            System.Console.WriteLine($"{Colours.RED}Invalid year input{Colours.NORMAL}");
            return;
        }

        var sql = @"SELECT account_id,
    SUM(amount) as total_income
    FROM transactions
    WHERE type = 'w'
    AND EXTRACT(DOW FROM date) = @weekDay
    AND EXTRACT(WEEK FROM date) = @week
    AND EXTRACT(YEAR FROM date) = @year
    AND account_id = @accountId::uuid  
    GROUP BY account_id";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@weekDay", dayInput);
        cmd.Parameters.AddWithValue("@week", weekInput);
        cmd.Parameters.AddWithValue("@year", yearInput);
        cmd.Parameters.AddWithValue("@accountId", accountId);



        try
        {
            using var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine($"{Colours.RED}No amount was withdrawn from the given timeline{Colours.NORMAL}");
                Console.ReadKey();
                System.Console.WriteLine();
                return;
            }

            while (reader.Read())
            {
                var totalIncome = reader.GetDecimal(1);
                Console.WriteLine($"Total spending made on day {dayInput} of week {weekInput} {yearInput}:{Colours.RED} {totalIncome * -1:c}{Colours.NORMAL}");
                System.Console.WriteLine();
                Console.ReadKey();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }


    }

    internal static void Run(string v)
    {
        throw new NotImplementedException();
    }
}