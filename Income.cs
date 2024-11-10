using System.Globalization;

public class Income
{


    private List<Transaction> transactions = new List<Transaction>();


    public Income(List<Transaction> transactions)
    {
        this.transactions = transactions;
    }


    public void YearIncome()
    {
        Console.Clear();
        decimal YearlyIncome = 0;

        System.Console.Write("Enter the year: ");
        int input = int.Parse(Console.ReadLine()!);
        System.Console.WriteLine();
        foreach (Transaction transaction in transactions)
        {
            if (transaction.TransactionType == "d" && transaction.Date.Year.Equals(input))
            {
                Console.WriteLine($"> Amount received:{Colours.GREEN} {transaction.Amount}{Colours.NORMAL}| Date: {Colours.BLUE}{transaction.Date}{Colours.NORMAL}");

                YearlyIncome += transaction.Amount;
            }
        }
        System.Console.WriteLine($"In {input} your total income was: {YearlyIncome}");
        System.Console.WriteLine();
        Console.ReadKey();
    }




    public void MonthlyIncome()
    {
        Console.Clear();

        decimal monthIncome = 0;

        System.Console.Write("Enter the month (1-12): ");
        int monthInput;
        if (!int.TryParse(Console.ReadLine()!, out monthInput) || monthInput < 1 || monthInput > 12)
        {
            System.Console.WriteLine("Invalid month input");
            return;
        }
        System.Console.Write("Enter the year: ");
        int yearInput = int.Parse(Console.ReadLine()!);

        string monthName = GetMonth(monthInput);

        foreach (Transaction transaction in transactions)
        {
            if (transaction.TransactionType == "d" && transaction.Date.Month.Equals(monthInput))
            {
                if (transaction.Date.Year.Equals(yearInput))
                {
                    Console.WriteLine($"> Amount received:{Colours.GREEN} {transaction.Amount}{Colours.NORMAL}| Date: {Colours.BLUE}{transaction.Date}{Colours.NORMAL}");
                    monthIncome += transaction.Amount;
                }
            }
        }
        System.Console.WriteLine($"In {monthName} {yearInput} your total income was {Colours.GREEN}{monthIncome}");
        System.Console.WriteLine($"{Colours.NORMAL}");
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
        Console.Clear();
        decimal weeklyIncome = 0;
        System.Console.Write("Enter week number (1-53): ");
        int weekInput;
        if (!int.TryParse(Console.ReadLine()!, out weekInput) || weekInput < 1 || weekInput > 52)
        {
            System.Console.WriteLine("Invalid week input");
            return;
        }
        System.Console.Write("Enter year: ");
        int YearInput = int.Parse(Console.ReadLine()!);

        Calendar calendar = CultureInfo.CurrentCulture.Calendar;
        CalendarWeekRule weekRule = CalendarWeekRule.FirstDay;
        DayOfWeek firstDayOfWeek = DayOfWeek.Monday;

        foreach (Transaction transaction in transactions)
        {
            if (transaction.TransactionType == "d" && transaction.Date.Year.Equals(YearInput))
            {
                int WeekOfTheYear = calendar.GetWeekOfYear(transaction.Date, weekRule, firstDayOfWeek);
                if (WeekOfTheYear == weekInput)
                {
                    Console.WriteLine($"> Amount received:{Colours.GREEN} {transaction.Amount}{Colours.NORMAL}| Date: {Colours.BLUE}{transaction.Date}{Colours.NORMAL}");
                    weeklyIncome += transaction.Amount;
                }
            }
        }
        System.Console.WriteLine($"In week {weekInput} of {YearInput} your total income was{Colours.GREEN}{weeklyIncome}");
        System.Console.WriteLine($"{Colours.NORMAL}");
        Console.ReadKey();

    }





    public void DailyIncome()
    {
        Console.Clear();

        decimal dayIncome = 0;

        System.Console.Write("Enter day of the week (Monday - Sunday): ");
        string dayInput = Console.ReadLine()!;
        if (!Enum.TryParse<DayOfWeek>(dayInput, true, out DayOfWeek dayOfWeek))
        {
            System.Console.WriteLine("Invalid weekday input");
            return;
        }


        System.Console.Write("Enter week number (1-53): ");
        int weekInput;
        if (!int.TryParse(Console.ReadLine()!, out weekInput) || weekInput < 1 || weekInput > 53)
        {
            System.Console.WriteLine("Invalid week input");
            return;
        }
        System.Console.Write("Enter year: ");
        int YearInput1 = int.Parse(Console.ReadLine()!);

        Calendar calendar = CultureInfo.CurrentCulture.Calendar;
        CalendarWeekRule weekRule = CalendarWeekRule.FirstDay;
        DayOfWeek firstDayOfWeek = DayOfWeek.Monday;


        foreach (Transaction transaction in transactions)
        {
            if (transaction.TransactionType == "d" && transaction.Date.Year.Equals(YearInput1))
            {
                int WeekOfTheYear = calendar.GetWeekOfYear(transaction.Date, weekRule, firstDayOfWeek);
                if (weekInput == WeekOfTheYear && transaction.Date.DayOfWeek.Equals(dayOfWeek))
                {
                    Console.WriteLine($"> Amount received:{Colours.GREEN} {transaction.Amount}{Colours.NORMAL}| Date: {Colours.BLUE}{transaction.Date}{Colours.NORMAL}");
                    dayIncome += transaction.Amount;
                }
            }
        }
        System.Console.WriteLine($"On {dayInput} week {weekInput} {YearInput1}, your total income was {Colours.GREEN}{dayIncome}{Colours.NORMAL}.  ");
        System.Console.WriteLine();
        Console.ReadKey();
    }
}