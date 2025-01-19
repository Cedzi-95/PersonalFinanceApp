using System.Globalization;
using Npgsql;
public class Income
{

    private NpgsqlConnection connection;
    private IUserService userService;





    public Income(NpgsqlConnection connection, IUserService userService)
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



    public void YearIncome()
    {
        var user = userService.GetLoggedInUser();
        var accountId = GetAccountIdForUser(user.UserId);


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
    WHERE type = 'd'
    AND EXTRACT(YEAR FROM date) = @year
    AND account_id = @accountId::uuid  -- Convert the parameter to UUID type
    GROUP BY account_id";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@year", yearInput);
        cmd.Parameters.AddWithValue("@accountId", accountId);



        try
        {
            using var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                Console.WriteLine($"{Colours.RED}No transactions found for year {yearInput}{Colours.NORMAL}");
                return;
            }

            while (reader.Read())
            {
                var totalIncome = reader.GetDecimal(1);    // assuming amount is decimal
                Console.WriteLine($"Total income in {yearInput}:{Colours.GREEN} {totalIncome:c}{Colours.NORMAL}");
                System.Console.WriteLine();
                Console.ReadKey();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }





    }





    public void MonthlyIncome()
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
    WHERE type = 'd'
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
                Console.WriteLine($"{Colours.RED}No income found from the given period. {Colours.NORMAL}");
                return;
            }

            while (reader.Read())
            {
                var totalIncome = reader.GetDecimal(1);
                Console.WriteLine($"Total income in {monthName} {InputYear} :{Colours.GREEN} {totalIncome:c}{Colours.NORMAL}");
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



    private string GetMonth(int month)
    {
        if (month < 1 || month > 12)
        {
            throw new ArgumentOutOfRangeException("Invalid month input");
        }
        DateTime date = new DateTime(2020, month, 1);
        return date.ToString("MMMM");
    }





    public void WeekIncome()
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
    WHERE type = 'd'
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
                Console.WriteLine($"{Colours.RED}No income was received from the given timeline{Colours.NORMAL}");
                return;
            }

            while (reader.Read())
            {
                var totalIncome = reader.GetDecimal(1);
                Console.WriteLine($"Total income in week {weekInput} {yearInput}:{Colours.GREEN} {totalIncome:c}{Colours.NORMAL}");
                System.Console.WriteLine();
                Console.ReadKey();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }




        // System.Console.WriteLine($"In week {weekInput} of {YearInput} your total income was{Colours.GREEN}{weeklyIncome}");
        // System.Console.WriteLine($"{Colours.NORMAL}");
        // Console.ReadKey();

    }





    public void DailyIncome()
    {
        Console.Clear();


        System.Console.Write("Enter day of the week (1 - 7): ");
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
            System.Console.WriteLine("Invalid year input");
            return;
        }

        var sql = @"SELECT account_id,
    SUM(amount) as total_income
    FROM transactions
    WHERE type = 'd'
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
                Console.WriteLine($"{Colours.RED}No income was received from the given timeline{Colours.NORMAL}");
                return;
            }

            while (reader.Read())
            {
                var totalIncome = reader.GetDecimal(1);
                Console.WriteLine($"Total income received on day {dayInput} of week {weekInput} {yearInput}:{Colours.GREEN} {totalIncome:c}{Colours.NORMAL}");
                System.Console.WriteLine();
                Console.ReadKey();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }


        // System.Console.Write("Enter week number (1-53): ");
        // int weekInput;
        // if (!int.TryParse(Console.ReadLine()!, out weekInput) || weekInput < 1 || weekInput > 53)
        // {
        //     System.Console.WriteLine("Invalid week input");
        //     return;
        // }
        // System.Console.Write("Enter year: ");
        // int YearInput1 = int.Parse(Console.ReadLine()!);

        // Calendar calendar = CultureInfo.CurrentCulture.Calendar;
        // CalendarWeekRule weekRule = CalendarWeekRule.FirstDay;
        // DayOfWeek firstDayOfWeek = DayOfWeek.Monday;


        // foreach (Transaction transaction in transactions)
        // {
        //     if (transaction.TransactionType == "d" && transaction.Date.Year.Equals(YearInput1))
        //     {
        //         int WeekOfTheYear = calendar.GetWeekOfYear(transaction.Date, weekRule, firstDayOfWeek);
        //         if (weekInput == WeekOfTheYear && transaction.Date.DayOfWeek.Equals(dayOfWeek))
        //         {
        //             Console.WriteLine($"> Amount received:{Colours.GREEN} {transaction.Amount}{Colours.NORMAL}| Date: {Colours.BLUE}{transaction.Date}{Colours.NORMAL}");
        //             dayIncome += transaction.Amount;
        //         }
        //     }
        // }
        // System.Console.WriteLine($"On {dayInput} week {weekInput} {YearInput1}, your total income was {Colours.GREEN}{dayIncome}{Colours.NORMAL}.  ");
        // System.Console.WriteLine();
        // Console.ReadKey();
    }
}