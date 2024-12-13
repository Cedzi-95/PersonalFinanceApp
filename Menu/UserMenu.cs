public class UserMenu : Menu
{
    public UserMenu(IUserService userService, ImenuService menuService, IaccountManager accountManager)
    {
        AddCommand(new CreateTransactionCommand(userService, menuService, accountManager));
        AddCommand (new TransactionListCommand(userService,menuService, accountManager));
        AddCommand(new DeleteTransactionsCommand(userService, menuService, accountManager));
        AddCommand(new LogoutCommand(userService, menuService, accountManager));
    }

    public override void Display()
    {
        System.Console.WriteLine("Type 'help' for a list of commands");
    }
}