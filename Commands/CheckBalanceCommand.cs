public class CheckBalanceCommand : Command
{
    public CheckBalanceCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base("balance", userService, menuService, accountManager)
    {

    }

    public override void Execute(string[] args)
    {
        Console.Clear();
        System.Console.WriteLine($"Current balance:{Colours.GREEN} {accountManager.CheckBalance():c} {Colours.NORMAL}");
        Console.WriteLine("\npress key to continue..");
        Console.ReadKey();
        menuService.SetMenu(new UserMenu(userService, menuService, accountManager));



    }
}

