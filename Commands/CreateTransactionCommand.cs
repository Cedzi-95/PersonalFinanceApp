public class CreateTransactionCommand : Command
{


    public CreateTransactionCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :  base 
    
    ("Create-transaction", " ", userService, menuService, accountManager)
    {

    }


    public override void Execute(string[] args)
    {
        accountManager.CreateTransaction();
        System.Console.WriteLine("Your transaction has gone through.");
    }


}