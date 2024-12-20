using System.Globalization;
using Newtonsoft.Json;
public class Account
{
    public Guid AccountId {get; init;}
    public decimal Balance { get; set; }

    private List<Transaction> transactions = new List<Transaction>();



   

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





   


    





}