public class IncomeStatsCommand : Command
{
    
    public IncomeStatsCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager):
    base ("Income", userService, menuService, accountManager) {}

    public override void Execute(string[] args)
    {
        accountManager.PrintIncome();
        Console.WriteLine("\npress key to continue..");
        Console.ReadKey();
        menuService.SetMenu(new UserMenu(userService, menuService, accountManager));

    }
}