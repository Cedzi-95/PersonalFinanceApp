public class LogoutCommand : Command
{
     public LogoutCommand (IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("logout", userService, menuService, accountManager) {}

    public override void Execute(string[] args)
    {
        userService.Logout();
    }
}