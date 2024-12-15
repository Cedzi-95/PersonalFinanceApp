public class LoginMenu : Menu 
{
    public LoginMenu (IUserService userService, ImenuService menuService, IaccountManager accountManager)
    {
        AddCommand(new LoginCommand(userService, menuService, accountManager));
        AddCommand(new RegisterUserCommand(userService, menuService, accountManager));
    }

    public override void Display()
    {
        System.Console.WriteLine("Welcome to your finance application. Type 'login' to login");
    }
}