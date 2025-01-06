public class CreateTransactionCommand : Command
{


    public CreateTransactionCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :  base 
    
    ("create-transaction", userService, menuService, accountManager)
    {

    }


    public override void Execute(string[] args)
    {
        Console.Clear();
        accountManager.CreateTransaction();
        System.Console.WriteLine("Your transaction has gone through.");
        Console.WriteLine("\npress key to continue..");
        Console.ReadKey();
        menuService.SetMenu(new UserMenu(userService, menuService, accountManager));

        
    }


}