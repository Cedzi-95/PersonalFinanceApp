public class HelpCommand : Command
{
    UserMenu userMenu;
    public HelpCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("help", userService, menuService, accountManager)
    {
        
    }

    public override void Execute(string[] args)
    {
       userMenu.Display();
    }
} 