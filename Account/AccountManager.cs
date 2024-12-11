public interface IaccountManager
{
    void loggin();
    void CreateTransaction();
    void CheckBalance();
    void PrintAllTransactions();
    void Deposition(decimal amount);
    void Withdraw(decimal amount);
    void DeleteTransactions();
    void PrintIncome();
    void PrintExpenditures();


}