public abstract class Menu
{
    private List<Command> commands = new List<Command>();

    

    public void AddCommand(Command command)
    {
        this.commands.Add(command);
    }


    public void ExecuteCommand(string inputCommand)
    {
        if(string.IsNullOrWhiteSpace(inputCommand))
        {
            throw new ArgumentException("Command cannot be emply");
        }

        string[] commandParts = inputCommand.Split(" ");
        string commandName = commandParts[0].ToLower();

        Command? matchningCommand = commands.FirstOrDefault(cmd => 
        cmd.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));


        if (matchningCommand != null)
        {
            matchningCommand.Execute(commandParts);
            return;
        }
        throw new ArgumentException($"command '{commandName}' not found. Type 'help' to see available commands!");
        // foreach (Command command in commands)
        // {
        //     if (command.Name.Equals(commandParts[0]))
        //     {
        //         command.Execute(commandParts);
        //         return;
        //     }
        // }

        throw new ArgumentException("Command not found.");

    }
    public abstract void Display();

    protected void DisplalAvailableCommands()
    {
        Console.WriteLine("\n Available commands");

        foreach(var command in commands)
        {
            Console.WriteLine($"-{command.Name}");
        }
        System.Console.WriteLine();
    }
}












































// using Npgsql;
// public class Menu
// {
//     private NpgsqlConnection connection;
    
//     public static void Execute()
//     {
//         // Account account = new Account();
//         PostgresAccount account = new PostgresAccount();



//         while (true)
//         {
//             try

//             {
//                 System.Console.WriteLine();
//                 Console.Clear();
//                 System.Console.WriteLine($"{Colours.GREEN}____WELCOME TO YOUR FINANCE APP____{Colours.NORMAL}");
//                 System.Console.WriteLine("\n Choose from the menu (1-9): ");
//                 System.Console.WriteLine("___________________________________");
//                 System.Console.WriteLine($"|[{Colours.GREEN}1{Colours.NORMAL}]- Make transaction            |");
//                 System.Console.WriteLine($"|[{Colours.GREEN}2{Colours.NORMAL}]- Check Balance               |");
//                 System.Console.WriteLine($"|[{Colours.GREEN}3{Colours.NORMAL}]- Transactions list           |");
//                 System.Console.WriteLine($"|[{Colours.GREEN}4{Colours.NORMAL}]- Save transactions to file.  |");
//                 System.Console.WriteLine($"|[{Colours.GREEN}5{Colours.NORMAL}]- Load transactions from file |");
//                 System.Console.WriteLine($"|[{Colours.GREEN}6{Colours.NORMAL}]- Income statistics           |");
//                 System.Console.WriteLine($"|[{Colours.GREEN}7{Colours.NORMAL}]- Expenditure statistics      |");
//                 System.Console.WriteLine($"|[{Colours.GREEN}8{Colours.NORMAL}]- Delete transactions         | ");
//                 System.Console.WriteLine($"|[{Colours.GREEN}9{Colours.NORMAL}]- Exit                        |");
//                 System.Console.WriteLine("|_________________________________|");
//                 int choice;
//                 if (!int.TryParse(Console.ReadLine()!, out choice))
//                 {
//                     System.Console.WriteLine($"{Colours.RED}Invalid choice{Colours.NORMAL}");
//                 }

//                 switch (choice)
//                 {
//                     case 1:
//                         PostgresAccount.CreateTransaction();
//                         break;
//                     case 2:
//                         account.CheckBalance();
//                         break;
//                     case 3:
//                         account.PrintAllTransactions();
//                         break;
//                     case 4:
//                         account.SaveToFile("accountData.Json");
//                         Thread.Sleep(3000);
//                         break;
//                     case 5:
//                         account.LoadFromFile("accountData.Json");
//                         Console.ReadKey();
//                         break;
//                     case 6: account.PrintIncome();
//                         break;
//                     case 7:
//                         account.PrintExpenditures();
//                         break;
//                     case 8:
//                         account.DeleteTransactionsFromFile("accountData.Json");
//                         Console.ReadKey();
//                         break;
//                     case 9:
//                         System.Console.WriteLine($"Are you sure you want to exit?  [{Colours.GREEN}YES{Colours.NORMAL}] or [{Colours.RED}NO{Colours.NORMAL}] ");
//                         string answer = Console.ReadLine()!.ToLower();
//                         if (answer == "no")
//                         {
//                             continue;
//                         }
//                         else if (answer == "yes")
//                         {
//                             System.Console.WriteLine("You´re exiting...");
//                             return;
//                         }
//                         return;


//                     default:
//                         System.Console.WriteLine("Invalid choice ");
//                         break;
//                 }

//             }
//             catch
//             {
//                 System.Console.WriteLine("this field can´t be null");
//             }

//         }

//     }
// }