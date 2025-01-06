public class IncomeCommand : Command
{
    public IncomeCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("Income", userService, menuService, accountManager)
    {

    }
    public override void Execute(string[] args)
    {
        Console.Clear();
        accountManager.PrintIncome();
    }
}