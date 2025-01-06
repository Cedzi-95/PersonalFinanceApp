public class TransactionListCommand : Command
{


    public TransactionListCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base("list", userService, menuService, accountManager)
    { }

    public override void Execute(string[] args)
    {
        List<Transaction> transactions = accountManager.PrintAllTransactions(userService.GetLoggedInUser().UserId);
        foreach (var transaction in transactions)
        {
            if (transaction != null)
            {
                System.Console.WriteLine(transaction);

            }
            else
            {
                System.Console.WriteLine("No transactions found");
            }
        }


        Console.WriteLine("\npress key to continue..");
        Console.ReadKey();
        menuService.SetMenu(new UserMenu(userService, menuService, accountManager));

    }

}