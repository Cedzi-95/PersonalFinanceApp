public class ExpenditureCommand : Command
{
     public ExpenditureCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("Spending", userService, menuService, accountManager)
    {

    }

    public override void Execute(string[] args)
    {
        Console.Clear();
        accountManager.PrintExpenditures();
        Console.WriteLine("\npress key to continue..");
        Console.ReadKey();
        menuService.SetMenu(new UserMenu(userService, menuService, accountManager));

    }
}