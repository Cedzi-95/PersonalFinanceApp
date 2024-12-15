public class HelpCommand : Command
{
    public HelpCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("help", userService, menuService, accountManager)
    {
        
    }

    public override void Execute(string[] args)
    {
       System.Console.WriteLine("");
    }
} 