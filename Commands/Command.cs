public abstract class Command
{
    public string Name {get; init;}
    public string Description {get; init;}
    protected IUserService userService;
    protected ImenuService menuService;
    protected IaccountManager accountManager;

    public Command(string name, string description, IUserService userService, ImenuService menuService, IaccountManager accountManager)
    {
        this.Name = name;
        this.Description = description;
        this.userService = userService;
        this.menuService = menuService;
        this.accountManager = accountManager;
    }
    public abstract void Execute(string[] args);

}