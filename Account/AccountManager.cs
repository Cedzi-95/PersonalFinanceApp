public interface IaccountManager
{
   
    void CreateTransaction();
    void CheckBalance();
    void PrintAllTransactions();
    void Deposition(decimal amount);
    void Withdraw(decimal amount);
    void DeleteTransactions();
    void PrintIncome();
    void PrintExpenditures();


}