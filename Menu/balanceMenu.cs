// public class BalanceMenu : Menu
// {
//   private readonly  IUserService userService;
//     private readonly ImenuService menuService;
//     private readonly IaccountManager accountManager;
//     public BalanceMenu (IUserService userService, ImenuService menuService, IaccountManager accountManager)
//     {
//          this.userService = userService;
//          this.menuService = menuService;
//          this.accountManager = accountManager;
//         AddCommand(new CheckBalanceCommand(userService, menuService, accountManager));
//     }

//     public override void Display()
//     {
//         System.Console.WriteLine("Type <check-balance> to view current balance.");
//     }
// }