using System.Globalization;

public class Income 
{
   
    
    private List<Transactions> transactions = new List<Transactions>();

    public Income(List<Transactions> transactions)
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
       foreach(Transactions transaction in transactions)
       {
        if(transaction.TransactionType == "d" && transaction.Date.Year.Equals(input))
        {       
            System.Console.WriteLine("> Amount received: " + transaction.Amount + " | date: " + transaction.Date);
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
        if(!int.TryParse(Console.ReadLine()!, out monthInput) || monthInput < 1 || monthInput > 12)
        {
            System.Console.WriteLine("Invalid month input");
            return;
        }
        System.Console.Write("Enter the year: ");
        int yearInput = int.Parse(Console.ReadLine()!);

        string monthName = GetMonth(monthInput);

    foreach(Transactions transaction in transactions)
    {
        if(transaction.TransactionType == "d" && transaction.Date.Month.Equals(monthInput))
        {
            if(transaction.Date.Year.Equals(yearInput))
            {
                System.Console.WriteLine("> Amount received: " + transaction.Amount + "| Date: " + transaction.Date);
                 monthIncome += transaction.Amount;
            }
        }
    }
    System.Console.WriteLine($"In {monthName} {yearInput} your total income was {monthIncome}");
    System.Console.WriteLine();
    Console.ReadKey();
       
    }
    private string GetMonth(int month)
    {
        if(month < 1 || month > 12)
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
        if(!int.TryParse(Console.ReadLine()!, out weekInput) || weekInput < 1 || weekInput > 52)
        {
            System.Console.WriteLine("Invalid week input");
            return;
        }
        System.Console.Write("Enter year: ");
        int YearInput = int.Parse(Console.ReadLine()!);

        Calendar calendar = CultureInfo.CurrentCulture.Calendar;
        CalendarWeekRule weekRule = CalendarWeekRule.FirstDay;
        DayOfWeek firstDayOfWeek = DayOfWeek.Monday;

        foreach(Transactions transaction in transactions)
        {
            if(transaction.TransactionType == "d" && transaction.Date.Year.Equals(YearInput))
            {
                int WeekOfTheYear = calendar.GetWeekOfYear(transaction.Date, weekRule, firstDayOfWeek);
                if(WeekOfTheYear == weekInput)
                {
                    System.Console.WriteLine("> Amount received: " + transaction.Amount + "| Date: " + transaction.Date);
                    weeklyIncome += transaction.Amount;
                }
            }
        }
        System.Console.WriteLine($"In week {weekInput} of {YearInput} your total income was {weeklyIncome}");
        System.Console.WriteLine();
        Console.ReadKey();

    }

    public void DailyIncome()
    {
        Console.Clear();

        decimal dayIncome = 0;

        System.Console.Write("Enter day of the week (Monday - Sunday): ");
        string dayInput = Console.ReadLine()!;
        if(!Enum.TryParse <DayOfWeek>(dayInput, true, out DayOfWeek dayOfWeek))
        {
            System.Console.WriteLine("Invalid weekday input");
            return;
        }
        

        System.Console.Write("Enter week number (1-53): ");
         int weekInput;
        if(!int.TryParse(Console.ReadLine()!, out weekInput) || weekInput < 1 || weekInput > 53)
        {
            System.Console.WriteLine("Invalid week input");
            return;
        }
        System.Console.Write("Enter year: ");
        int YearInput1 = int.Parse(Console.ReadLine()!);

        Calendar calendar = CultureInfo.CurrentCulture.Calendar;
        CalendarWeekRule weekRule = CalendarWeekRule.FirstDay;
        DayOfWeek firstDayOfWeek = DayOfWeek.Monday;


        foreach(Transactions transaction in transactions)
        {
            if(transaction.TransactionType == "d" && transaction.Date.Year.Equals(YearInput1))
            {
                int WeekOfTheYear = calendar.GetWeekOfYear(transaction.Date, weekRule, firstDayOfWeek);
                if(weekInput == WeekOfTheYear && transaction.Date.DayOfWeek.Equals(dayOfWeek))
                {
                    System.Console.WriteLine("> Amount received: " + transaction.Amount + "| Date: " + transaction.Date);
                    dayIncome += transaction.Amount;
                }
            }
        }
        System.Console.WriteLine($"On {dayInput} week {weekInput} {YearInput1}, your total income was {dayIncome}.  ");
        System.Console.WriteLine();
        Console.ReadKey();
    }
}