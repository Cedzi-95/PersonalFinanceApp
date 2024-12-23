public class IncomeStatsCommand : Command
{
    
    public IncomeStatsCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager):
    base ("Income-stats", userService, menuService, accountManager) {}

    public override void Execute(string[] args)
    {
        accountManager.PrintIncome();
    }
}