public class IncomeCommand : Command
{
    public IncomeCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("Income", "view income", userService, menuService, accountManager)
    {

    }
    public override void Execute(string[] args)
    {
        accountManager.PrintIncome();
    }
}