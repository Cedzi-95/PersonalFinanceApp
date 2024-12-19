public interface IaccountManager
{

   
    void CreateTransaction();
    decimal CheckBalance();
    List<Transaction> PrintAllTransactions(Guid accountId);
    void DeleteTransactions();
    void PrintIncome();
    void PrintExpenditures();
}