using System.Globalization;

public class Account
{
    private decimal balance {get; set;}
    private List<Transactions> transactions = new List<Transactions>();

    public void CreateTransactions()
    {
        


       System.Console.WriteLine("Enter transaction type: press 'd' for Deposition and 'w' for Withdraw ");
     string transactionInput = Console.ReadLine()!;
    //  if(transactionInput != "deposition" || transactionInput != "withdraw")
    //  {
    //     System.Console.WriteLine("Invalid transaction type, please enter 'deposition' or 'withdraw'");
    //  }

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

        
        System.Console.WriteLine("Press key to go back to menu");
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
        System.Console.WriteLine("\n Balance: "+ balance);
        System.Console.WriteLine("\n press key to continiue..");
        Console.ReadKey();
    }
    private void Deposition(decimal amount)
    {
        balance += amount;
        System.Console.WriteLine($" Deposition amount: {amount} \n New balance: {balance}");
        System.Console.WriteLine("\n Press key to continue...");
        Console.ReadKey();
    }


    private void Withdraw(decimal amount)
    {
        if(amount > balance)
        {
            System.Console.WriteLine("Insufficient funds");
             System.Console.WriteLine("\n Press key to continue...");
        Console.ReadKey();
        }
        else 
       {
         balance -= amount;
      System.Console.WriteLine($" Amount retreived: {amount} \n New balance: {balance}");
       System.Console.WriteLine("\n Press key to continue...");
        Console.ReadKey();
      }
    }
    public void YearOfTransactions()
    {
        System.Console.WriteLine("Enter the year: ");
        string Input = Console.ReadLine()!;
        IEnumerable<Transactions>timeline = transactions
        .Where((transaction) => transaction.Date.Year.ToString().Equals(Input));
        foreach(Transactions transaction in timeline)
        {
            System.Console.WriteLine(transaction);

        }
Console.ReadKey();
    
    }
    public void IncomeStats()
    {
        decimal YearlyIncome = 0;

        System.Console.Write("Enter the year you want to view your income stats: ");
        int input = int.Parse(Console.ReadLine()!);
       foreach(Transactions transaction in transactions)
       {
        if(transaction.TransactionType == "d" && transaction.Date.Year.Equals(input))
        {       
            System.Console.WriteLine("> Amount received: " + transaction.Amount + " | date: " + transaction.Date);
        }
       }
       foreach(Transactions transaction in transactions)
       {
        if(transaction.TransactionType == "d" && transaction.Date.Year.Equals(input))
        {
                    YearlyIncome += transaction.Amount;
        }
       }
       System.Console.WriteLine($"In {input} your net income was: {YearlyIncome}");
       Console.ReadLine();


    }
    public void SpendingStats()
    {
        decimal YearlySpending = 0;

        System.Console.Write("Enter the year you want to view your spendings stats: ");
        int yearInput = int.Parse(Console.ReadLine()!);
        foreach(Transactions transaction in transactions)
        {
            if(transaction.TransactionType == "w" && transaction.Date.Year.Equals(yearInput))
            {
                System.Console.WriteLine(">Amount spent: -" + transaction.Amount + " | date: "+ transaction.Date);
            }
        }
        foreach(Transactions transaction in transactions)
        {
             if(transaction.TransactionType == "w" && transaction.Date.Year.Equals(yearInput))
             {
                YearlySpending -= transaction.Amount;
             }
        }
        System.Console.WriteLine($"In {yearInput} your total expenditure was {YearlySpending}");
        System.Console.ReadKey();
    }
}