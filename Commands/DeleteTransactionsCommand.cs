public class DeleteTransactionsCommand : Command
{
    public DeleteTransactionsCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("delete", userService, menuService,accountManager) {}

    public override void Execute(string[] args)
    {
        Console.Clear();
        accountManager.DeleteTransactions();
        Console.WriteLine("\npress key to continue..");
        Console.ReadKey();
        menuService.SetMenu(new UserMenu(userService, menuService, accountManager));

    }
}