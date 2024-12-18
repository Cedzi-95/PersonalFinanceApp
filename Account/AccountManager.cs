public interface IaccountManager
{

   
    void CreateTransaction();
    void CheckBalance();
    List<Transaction> PrintAllTransactions(Guid accountId);
    void DeleteTransactions();
    void PrintIncome();
    void PrintExpenditures();
}