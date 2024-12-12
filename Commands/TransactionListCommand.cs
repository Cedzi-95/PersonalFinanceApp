public class TransactionListCommand : Command
{
    public TransactionListCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("transaction-list", "get all transactions", userService, menuService, accountManager) {}

    public override void Execute(string[] args)
    {
        accountManager.PrintAllTransactions();  //jag kanske måste ändra hör sen om det krånglar. vet inte än om det alla accounts eller enbart list
        //för användare

        
    }

}