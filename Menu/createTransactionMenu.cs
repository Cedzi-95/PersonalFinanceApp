public class CreateTransactionMenu : Menu
{
    Account account {get; init;}
    IUserService userService;
    ImenuService menuService;
    IaccountManager accountManager;
    public CreateTransactionMenu (IUserService userService, ImenuService menuService, IaccountManager accountManager)
    {
         this.userService = userService;
         this.menuService = menuService;
         this.accountManager = accountManager;
        AddCommand(new CreateTransactionCommand(userService, menuService, accountManager));
    }

    public override void Display()
    {
        System.Console.WriteLine("Type <create-transaction> to make transactions.");
    }
}