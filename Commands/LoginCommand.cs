public class LoginCommand : Command
{
     public LoginCommand(IUserService userService, ImenuService menuService, IaccountManager accountManger) : base
     ("Login", "Login with username and password.", userService, menuService, accountManger) {}

    public override void Execute(string[] args)
    {
        string username = args[1];
        string password = args[2];

        User? user = userService.login(username, password);

        if (user == null)
        {
            System.Console.WriteLine("Wrong username or password.");
            return;
        }
        System.Console.WriteLine("You have successfully logged in");
        menuService.SetMenu(new UserMenu(userService, menuService, accountManager));
    }

}