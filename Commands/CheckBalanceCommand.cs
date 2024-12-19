public class CheckBalanceCommand : Command
{
    public CheckBalanceCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("balance", userService, menuService, accountManager)
    {

    }

    public override void Execute(string[] args)
    {
        //accountManager.CheckBalance();
        System.Console.WriteLine($"Current balance: {accountManager.CheckBalance():c}");
    }
}

