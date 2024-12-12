using System.Globalization;
using Newtonsoft.Json;
public class Account
{
    public Guid AccountId {get; init;}
    public decimal Balance { get; set; }

    private List<Transaction> transactions = new List<Transaction>();



    public void CreateTransaction()
    {

      



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



    public void CheckBalance()
    {
        System.Console.WriteLine($"\n{Colours.GREEN} Current balance:{Colours.GREEN} {Balance}");
        System.Console.WriteLine($"{Colours.NORMAL}");
        System.Console.WriteLine("\n press key to continiue..");
        Console.ReadKey();
    }




    private void Deposition(decimal amount)
    {
        Console.Clear();
        Balance += amount;
        System.Console.WriteLine($" Deposition amount: {Colours.GREEN} {amount} {Colours.NORMAL} \n New balance:{Colours.GREEN} {Balance} {Colours.NORMAL}");
        System.Console.WriteLine("\n Press key to continue...");
        Console.ReadKey();
    }




    private void Withdraw(decimal amount)
    {
        Console.Clear();
        if (amount <= Balance)
        {
           
            Balance -= amount;
            System.Console.WriteLine($"> Amount retreived:{Colours.RED} {amount} {Colours.NORMAL} \n{Colours.GREEN}New balance: {Balance}");
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




    //SAVING TO FILE
    public void SaveToFile(string filePath)
    {
        try
        {
            var accountData = new
            {
                Balance,
                Transactions = transactions
            };

            string jsonData = JsonConvert.SerializeObject(accountData, Formatting.Indented); //encoding data to json file
            File.WriteAllText(filePath, jsonData);
            System.Console.WriteLine($"{Colours.GREEN}Data saved successfully!{Colours.NORMAL}");
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"{Colours.RED}Error saving data: {e} {Colours.NORMAL}");
        }
    }




    public void LoadFromFile(string filePath) //loading from file
    {
        try
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                System.Console.WriteLine("Raw data :\n" + jsonData); //display data from json file

                var accountData = JsonConvert.DeserializeObject<dynamic>(jsonData);

                System.Console.WriteLine($"{Colours.GREEN}Data loaded successfully");
                System.Console.WriteLine($"{Colours.NORMAL}");
            }
            else
            {
                System.Console.WriteLine($"{Colours.RED}File not found! {Colours.NORMAL}");
            }
        }
        catch (Exception e)
        {
            System.Console.WriteLine($"Error loading data: {e}");
        }
    }




    public void DeleteTransactionsFromFile(string filePath)
    {

        System.Console.WriteLine("Transactions to delete: [1] All transactions  [2] Single transaction");
        int deleteTrans;
        if (!int.TryParse(Console.ReadLine()!, out deleteTrans))
        {
            System.Console.WriteLine($"{Colours.RED} Invalid input! {Colours.NORMAL}");
            return;
        }
        if (deleteTrans == 1)
        {
            transactions.Clear();
            System.Console.WriteLine($"{Colours.RED} All transactions deleted. {Colours.NORMAL}");
        }

        else if (deleteTrans == 2)
        {
            System.Console.WriteLine("Enter index of the transactions to delete: (index must be equal or superior to 0)");
            int index;
            if (!int.TryParse(Console.ReadLine()!, out index) || index < 0 || index >= transactions.Count)
            {
                System.Console.WriteLine($"{Colours.RED} Invalid index input! {Colours.NORMAL}");
                return;
            }
            transactions.RemoveAt(index);
            System.Console.WriteLine($"{Colours.GREEN} chosen transactions are deleted {Colours.NORMAL}");
        }
        else
        {
            System.Console.WriteLine($"{Colours.RED} Invalid Choice {Colours.NORMAL}");
        }
        SaveToFile(filePath);

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
}