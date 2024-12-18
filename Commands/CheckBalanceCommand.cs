public class CheckBalanceCommand : Command
{
    public CheckBalanceCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) :
    base ("balance", userService, menuService, accountManager)
    {

    }

    public override void Execute(string[] args)
    {
        accountManager.CheckBalance();
    }
}

// // Command to switch to Balance Menu
// public class SwitchToBalanceCommand : Command
// {
//     public SwitchToBalanceCommand(IUserService userService, ImenuService menuService, IaccountManager accountManager) 
//         : base("balance", userService, menuService, accountManager)
//     {
//     }

//     public override void Execute(string[] args)
//     {
//          var user = userService.GetLoggedInUser();
//         if (user == null)
//         {
//             throw new InvalidOperationException("You must be logged in to access the balance menu.");
//         }

//         var balanceMenu = new BalanceMenu(userService, menuService, accountManager);
//         menuService.SetMenu(balanceMenu);
//     }
// }