public class SpendingStatsCommand : Command
{
    public SpendingStatsCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager):
    base ("Spending-stats", userService, menuService, accountManager)
    {
        
    }

    public override void Execute(string[] args)
    {
       accountManager.PrintExpenditures();
    }
}