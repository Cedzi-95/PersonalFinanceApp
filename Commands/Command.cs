public abstract class Command
{
    public string Name {get; init;}
    
    protected IUserService userService;
    protected ImenuService menuService;
    protected IaccountManager accountManager;

    public Command(string name, IUserService userService, ImenuService menuService, IaccountManager accountManager)
    {
        this.Name = name;
       
        this.userService = userService;
        this.menuService = menuService;
        this.accountManager = accountManager;
    }
    public abstract void Execute(string[] args);

}