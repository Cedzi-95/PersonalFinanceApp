public class UserMenu : Menu
{
    private IUserService userService;
    private ImenuService menuService;
    private IaccountManager accountManager;

    public UserMenu(IUserService userService, ImenuService menuService, IaccountManager accountManager)
    {
        AddCommand(new CreateTransactionCommand(userService, menuService, accountManager));
        AddCommand (new TransactionListCommand(userService,menuService, accountManager));
        AddCommand(new DeleteTransactionsCommand(userService, menuService, accountManager));
        AddCommand(new LogoutCommand(userService, menuService, accountManager));
        AddCommand(new CheckBalanceCommand(userService, menuService, accountManager));
        AddCommand(new IncomeStatsCommand(userService, menuService, accountManager));
        AddCommand(new SpendingStatsCommand(userService, menuService, accountManager));
        AddCommand(new HelpCommand(userService, menuService, accountManager));
    }


    public override void Display()
    {
        // System.Console.WriteLine($"Type 'help' to view all the commands");
        Console.Clear();
        System.Console.WriteLine($"{Colours.NORMAL}");
        System.Console.WriteLine("Type 'help' for a list of commands");
        System.Console.WriteLine();
        System.Console.WriteLine($"\n{Colours.GREEN}[Create-transaction]{Colours.NORMAL} for deposition or widrawal ");
        System.Console.WriteLine($"\n{Colours.GREEN}[List]{Colours.NORMAL} to view all your transactions ");
        System.Console.WriteLine($"\n{Colours.GREEN}[Balance]{Colours.NORMAL} to view your current balance ");
        System.Console.WriteLine($"\n{Colours.GREEN}[Income]{Colours.NORMAL} to view income statistics");
        System.Console.WriteLine($"\n{Colours.GREEN}[Spending]{Colours.NORMAL} to view spending statistics");
        System.Console.WriteLine($"\n{Colours.GREEN}[Delete]{Colours.NORMAL} to remove transactions");
        System.Console.WriteLine($"\n{Colours.RED}[Logout]{Colours.NORMAL} to exit the app ");
    }
}