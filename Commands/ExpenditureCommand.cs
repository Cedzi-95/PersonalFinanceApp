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
    }
}