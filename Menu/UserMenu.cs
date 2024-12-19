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
    }


    public override void Display()
    {
        Console.Clear();
        System.Console.WriteLine($"{Colours.NORMAL}");
        System.Console.WriteLine("Type 'help' for a list of commands");
        System.Console.WriteLine();
        System.Console.WriteLine($"Type <{Colours.GREEN}Create-transaction{Colours.NORMAL}> for deposition or widrawal ");
        System.Console.WriteLine($"Type <{Colours.GREEN}List-transactions{Colours.NORMAL}> to view all your transactions ");
        System.Console.WriteLine($"Type <{Colours.GREEN}Balance{Colours.NORMAL}> to view your current balance ");
        System.Console.WriteLine($"Type <{Colours.RED}Exit{Colours.NORMAL}> to exit the app ");
    }
}