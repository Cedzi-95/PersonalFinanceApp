public class LoginCommand : Command
{
     public LoginCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) : base
     ("login", userService, menuService, accountManager) {}

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
        Console.Clear();
        System.Console.WriteLine($"{Colours.GREEN}You have successfully logged in!{Colours.NORMAL}");
        Console.WriteLine();
        menuService.SetMenu(new UserMenu(userService, menuService, accountManager));
    }

}