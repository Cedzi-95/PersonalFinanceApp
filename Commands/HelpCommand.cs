public class HelpCommand : Command
{
        public HelpCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("help", userService, menuService, accountManager)
    {
        
    }

    public override void Execute(string[] args)
    {
       Console.Clear();
        System.Console.WriteLine($"{Colours.NORMAL}");
        System.Console.WriteLine("Type 'help' for a list of commands");
        System.Console.WriteLine();
        System.Console.WriteLine($"\n{Colours.GREEN}[Create-transaction]{Colours.NORMAL} for deposition or widrawal ");
        System.Console.WriteLine($"\n{Colours.GREEN}[List-transactions]{Colours.NORMAL} to view all your transactions ");
        System.Console.WriteLine($"\n{Colours.GREEN}[Balance]{Colours.NORMAL} to view your current balance ");
        System.Console.WriteLine($"\n{Colours.GREEN}[Income-stats{Colours.NORMAL} to view income statistics");
        System.Console.WriteLine($"\n{Colours.GREEN}[Spending-stats{Colours.NORMAL} to view spending statistics");
        System.Console.WriteLine($"\n{Colours.GREEN}[Delete]{Colours.NORMAL} to remove transactions");
        System.Console.WriteLine($"\n{Colours.RED}[Exit]{Colours.NORMAL} to exit the app ");
    }
} 