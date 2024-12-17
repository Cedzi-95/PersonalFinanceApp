public class TransactionListCommand : Command
{
    
    
    public TransactionListCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("list-transactions", userService, menuService, accountManager) {}

    public override void Execute(string[] args)
    {
        List<Transaction> transactions = accountManager.PrintAllTransactions();
        foreach (var transaction in transactions)
        {
            if(transaction != null)
            {
                System.Console.WriteLine(transaction);
            }
            else
            {
                System.Console.WriteLine("No transactions found");
            }
        }
        


    }

}