using System.Globalization;

public class Account
{
    private decimal balance {get; set;}
    private List<Transactions> transactions = new List<Transactions>();

    public void CreateTransactions()
    {

        Console.Clear();
       System.Console.WriteLine("Enter transaction type: press 'd' for Deposition and 'w' for Withdraw ");
     string transactionInput = Console.ReadLine()!;
     
         System.Console.WriteLine("Enter amount: ");
     decimal amount;
  
        if(!decimal.TryParse(Console.ReadLine()!, out amount) || amount <= 0)
        {
            System.Console.WriteLine("Invalid amount Please enter positive amount");
            return;
        }
 System.Console.WriteLine("Enter date: (yyyy-MM-dd) ");
        string dateInput = Console.ReadLine()!;
        DateTime date;
        try
       {
         date = DateTime.ParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture);
       }
       catch(FormatException)
       {
        throw new ArgumentException("Invalid date input, please enter the date in the correct format");
        
       }
     
     Transactions transaction = new Transactions
     {
        Date = date,
        TransactionType = transactionInput,
        Balance = balance,
        Amount = amount, 
     };
     

    if(transactionInput.Equals("d"))
    {
            Deposition(amount);
    }
    else if (transactionInput.Equals("w"))
    {
        Withdraw(amount);
    }
    else
    {
        System.Console.WriteLine("Invalid transaction type");
        return;
    }

    transactions.Add(transaction);
    System.Console.WriteLine();
     System.Console.WriteLine(transaction.ToString());
     


    }
    public void GetAllTransactions()
    {
    System.Console.WriteLine("List of all the transactions ");
        System.Console.WriteLine();
        foreach(Transactions transaction in transactions)
        {
           System.Console.WriteLine(transaction.ToString());
           System.Console.WriteLine();
        }

        
        Console.ReadKey();

    }



    public void RemoveTransactions()
    {
        System.Console.WriteLine("Transaction deleted");
     for(int i = 0; i < transactions.Count; i++)
     {
        transactions.Remove(transactions[i]);

     }     
        
    }



    public void CheckBalance()
    {
        System.Console.WriteLine("\n Current balance: " + balance);
        System.Console.WriteLine("\n press key to continiue..");
        Console.ReadKey();
    }


    private void Deposition(decimal amount)
    {
        Console.Clear();
        balance += amount;
        System.Console.WriteLine($" Deposition amount: {amount} \n New balance: {balance}");
        System.Console.WriteLine("\n Press key to continue...");
        Console.ReadKey();
    }


    private void Withdraw(decimal amount)
    {
        Console.Clear();
        if(amount > balance)
        {
            System.Console.WriteLine("Insufficient funds");
             System.Console.WriteLine("\n Press key to continue...");
        Console.ReadKey();
        }
        else 
       {
         balance -= amount;
      System.Console.WriteLine($"> Amount retreived: {amount} \n New balance: {balance}");
       System.Console.WriteLine("\n Press key to continue...");
        Console.ReadKey();
      }
    }


    public void Income()
    {
         Income income = new Income(transactions);
    try
    {
        while (true)
        {
    Console.WriteLine("Type one of the alternatives below to view income stats :\n 'year' 'month' 'week' 'day' 'menu' ");
    string myChoice = Console.ReadLine()!.ToLower();

    switch(myChoice)
    {
        case "year": income.YearIncome();
        break;
        case "month": income.MonthlyIncome();
        break;
        case "week": income.WeekIncome();
        break;
        case "day": income.DailyIncome();
        break;
        
        case "menu": System.Console.WriteLine("Back to the menu.");
        return;
    }

           

            
        }
    }
    catch
    {
        throw new ArgumentException("Invalid input");
    }
   

    }


    public void SpendingStats()
    {
        Expenditure expenditure = new Expenditure(transactions);

        try
        {
            while(true)
            {
            Console.WriteLine("Type one of the alternatives below to view spending stats :\n 'year' 'month' 'week' 'day' 'exit' ");
            string myChoice = Console.ReadLine()!.ToLower();

            switch(myChoice)
            {
                case "year": expenditure.AnnualSpending();
                break;
                case "month": expenditure.MonthSpend();
                break;
                case "week": expenditure.WeeklyExpenditure();
                break;
                case "day":  expenditure.DailySpending();
                break;
                case "exit": Console.WriteLine("back to the meny...");
                return;

            }
            }
        }
        catch
        {
            throw new ArgumentNullException("Invalid input");
        }
    }
}