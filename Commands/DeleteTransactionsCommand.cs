public class DeleteTransactionsCommand : Command
{
    public DeleteTransactionsCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("delete-transactions", "delete all transactions", userService, menuService,accountManager) {}

    public override void Execute(string[] args)
    {
        accountManager.DeleteTransactions();
    }
}