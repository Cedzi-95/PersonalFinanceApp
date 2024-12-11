using System.Globalization;

public class Expenditure
{

    private List<Transaction> transactions = new List<Transaction>();

    public Expenditure(List<Transaction> transactions)
    {
        this.transactions = transactions;
    }
   

    public void AnnualSpending()
    {
        decimal YearlySpending = 0;

        System.Console.Write("Enter the year: ");
        int yearInput = int.Parse(Console.ReadLine()!);
        foreach (Transaction transaction in transactions)
        {
            if (transaction.TransactionType == "w" && transaction.Date.Year.Equals(yearInput))
            {
                Console.WriteLine($"> Amount spent:{Colours.RED}{transaction.Amount}{Colours.NORMAL} | Date:{Colours.BLUE}{transaction.Date}");
                System.Console.WriteLine($"{Colours.NORMAL}");
                YearlySpending -= transaction.Amount;
            }
        }

        System.Console.WriteLine($"In {yearInput} your total expenditure was {Colours.RED}{YearlySpending * -1}");
        System.Console.WriteLine($"{Colours.NORMAL}");
        System.Console.ReadKey();
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
        decimal monthlySpending = 0;

        System.Console.Write("Enter month (1-12): ");
        int monthInput;
        if (!int.TryParse(Console.ReadLine()!, out monthInput) || monthInput < 1 || monthInput > 12)
        {
            throw new ArgumentOutOfRangeException("Invalid month input");
        }
        System.Console.Write("Enter year: ");
        int yearInput = int.Parse(Console.ReadLine()!);

        string month = GetMonth(monthInput);

        foreach (Transaction transaction in transactions)
        {

            if (transaction.TransactionType == "w" && transaction.Date.Month.Equals(monthInput))
            {
                if (transaction.Date.Year.Equals(yearInput))
                {
                    Console.WriteLine($"> Amount spent: {Colours.RED}{transaction.Amount}{Colours.NORMAL} | Date:{Colours.BLUE}{transaction.Date}");
                    System.Console.WriteLine($"{Colours.NORMAL}");
                    monthlySpending -= transaction.Amount;
                }
            }

        }

        System.Console.WriteLine($"In {month} {yearInput} your total expenditure was {monthlySpending * -1} ");
        Console.WriteLine();
        Console.ReadKey();

    }







    public void WeeklyExpenditure()
    {
        decimal weeklySpending = 0;

        System.Console.Write("Enter week number (1-53): ");
        int weekNumInput;
        if (!int.TryParse(Console.ReadLine()!, out weekNumInput) || weekNumInput < 1 || weekNumInput > 53)
        {
            throw new ArgumentOutOfRangeException("Invalid week input");
        }

        System.Console.Write("Enter the year: ");
        int yearInput = int.Parse(Console.ReadLine()!);

        Calendar calendar = CultureInfo.CurrentCulture.Calendar;
        CalendarWeekRule weekRule = CalendarWeekRule.FirstDay;
        DayOfWeek firstDayOfWeek = DayOfWeek.Monday;

        foreach (Transaction transaction in transactions)
        {
            if (transaction.TransactionType == "w" && transaction.Date.Year.Equals(yearInput))
            {
                int WeekOfTheYear = calendar.GetWeekOfYear(transaction.Date, weekRule, firstDayOfWeek);
                if (WeekOfTheYear == weekNumInput)
                {
                    Console.WriteLine($"> Amount spent: {Colours.RED}{transaction.Amount}{Colours.NORMAL} | Date:{Colours.BLUE}{transaction.Date}");
                    System.Console.WriteLine($"{Colours.NORMAL}");
                    weeklySpending -= transaction.Amount;
                }
            }
        }
        System.Console.WriteLine($"In week {weekNumInput} of {yearInput} your total expenditure was{Colours.RED} {weeklySpending * -1} ");
        System.Console.WriteLine($"{Colours.NORMAL}");
        Console.ReadKey();
    }






    public void DailySpending()
    {

        decimal daySpending = 0;

        System.Console.Write("Enter day of the week (Monday-Sunday): ");
        string dayInput = Console.ReadLine()!;
        if (!Enum.TryParse<DayOfWeek>(dayInput, true, out DayOfWeek dayOfWeek))
        {
            System.Console.WriteLine("Invalid weekday input");
            return;
        }

        System.Console.Write("Enter week of the year (1-53): ");
        int weekYearInput;
        if (!int.TryParse(Console.ReadLine()!, out weekYearInput) || weekYearInput < 1 || weekYearInput > 53)
        {
            System.Console.WriteLine("Invalid week input");
            return;
        }

        Calendar calendar = CultureInfo.CurrentCulture.Calendar;
        CalendarWeekRule weekRule = CalendarWeekRule.FirstDay;
        DayOfWeek firstDayOfWeek = DayOfWeek.Monday;

        System.Console.Write("Enter year:");
        int yearInput = int.Parse(Console.ReadLine()!);

        foreach (Transaction transaction in transactions)
        {
            if (transaction.TransactionType == "w" && transaction.Date.Year.Equals(yearInput))
            {
                int weekYear = calendar.GetWeekOfYear(transaction.Date, weekRule, firstDayOfWeek);
                if (weekYearInput == weekYear && transaction.Date.DayOfWeek.Equals(dayOfWeek))
                {
                    Console.WriteLine($"> Amount spent: {Colours.RED}{transaction.Amount}{Colours.NORMAL} | Date:{Colours.BLUE}{transaction.Date}");
                    System.Console.WriteLine($"{Colours.NORMAL}");
                    daySpending -= transaction.Amount;
                }
            }
        }
        System.Console.WriteLine($"On {dayInput} week {weekYearInput} of {yearInput} your total expenditure was {Colours.RED} {daySpending * -1}");
        System.Console.WriteLine($"{Colours.NORMAL}");
        Console.ReadKey();



    }

    internal static void Run(string v)
    {
        throw new NotImplementedException();
    }
}