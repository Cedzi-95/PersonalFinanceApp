public class Menu
{
    public static void Execute()
    {
        Account account = new Account();
        
       while(true)
        {
            // Console.Clear();
            try

{           System.Console.WriteLine();
Console.Clear();
    System.Console.WriteLine("____WELCOME TO YOUR FINANCE APP____");
     System.Console.WriteLine("\n Choose from the menu: ");
    System.Console.WriteLine();
            System.Console.WriteLine("1. Make transaction ");
            System.Console.WriteLine("2. Check Balance ");
            System.Console.WriteLine("3. Transactions list ");
            System.Console.WriteLine("4. Income statistics");
            System.Console.WriteLine("5. Expenditure statistics");
            System.Console.WriteLine("6. Delete transactions ");
            System.Console.WriteLine("8. Exit ");
            System.Console.WriteLine();
            int choice = int.Parse(Console.ReadLine()!);
            if(choice.Equals(null))
            {
                throw new ArgumentException("Please choose from the menu.");
            }
            switch(choice)
            {
                case 1: account.CreateTransactions();
                break;
                case 2: account.CheckBalance();
                break;
                case 3: account.GetAllTransactions();
                break;
                case 4: account.Income();
                break;
                case 5: account.SpendingStats();
                break;
                case 6: account.RemoveTransactions();
                break;
                case 8: System.Console.WriteLine("You are exiting...");
                return;
                 default: System.Console.WriteLine("Invalid choice ");
                break;
            }
} catch
{
    System.Console.WriteLine("this field cant be null");
}    

    }

}
}