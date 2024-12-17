public class TransactionsListMenu : Menu
{
    Account account {get; init;}
    IUserService userService;
    ImenuService menuService;
    IaccountManager accountManager;
    public TransactionsListMenu (IUserService userService, ImenuService menuService, IaccountManager accountManager)
    {
         this.userService = userService;
         this.menuService = menuService;
         this.accountManager = accountManager;
        AddCommand(new TransactionListCommand(userService, menuService, accountManager));
    }

    public override void Display()
    {
        System.Console.WriteLine("Type <list-transactions> to make transactions.");
    }
}