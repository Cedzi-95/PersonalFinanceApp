public class LogoutCommand : Command
{
     public LogoutCommand (IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("logout", userService, menuService, accountManager) {}

    public override void Execute(string[] args)
    {
       System.Console.WriteLine(userService.Logout());
       menuService.SetMenu(new LoginMenu(userService, menuService, accountManager));
    }
}