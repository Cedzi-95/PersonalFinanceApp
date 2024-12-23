public class LoginMenu : Menu 
{
    public LoginMenu (IUserService userService, ImenuService menuService, IaccountManager accountManager)
    {
        AddCommand(new LoginCommand(userService, menuService, accountManager));
        AddCommand(new RegisterUserCommand(userService, menuService, accountManager));
        
    }

    public override void Display()
    {
        Console.WriteLine($"        \n{Colours.GREEN} Welcome to Personal Finance App{Colours.NORMAL}  ");
        Console.WriteLine("||-------------------------------");
        Console.WriteLine($"||{Colours.GREEN}login{Colours.NORMAL} <username> <password> - Log into your account");
        Console.WriteLine($"||{Colours.GREEN}register{Colours.NORMAL} <username> <password> - Create a new account");
        Console.WriteLine("---------------------------------");
    }
}