public class RegisterUserCommand : Command
{
    public RegisterUserCommand (IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("register", userService, menuService, accountManager) {}
public override void Execute(string[] args)
    {
       string username = args[1];
       string password = args[2];

       User user = userService.RegisterUser(username, password);

       System.Console.WriteLine($"Created user '{user.Name}'");
    }
}