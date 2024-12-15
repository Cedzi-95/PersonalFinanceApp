public class CheckBalanceCommand : Command
{
    public CheckBalanceCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("check-balance", userService, menuService, accountManager)
    {

    }

    public override void Execute(string[] args)
    {
        accountManager.CheckBalance();
    }
}