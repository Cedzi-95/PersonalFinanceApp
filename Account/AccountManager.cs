public interface IaccountManager
{

   
    void CreateTransaction();
    decimal CheckBalance();
    List<Transaction> PrintAllTransactions(Guid accountId);
     Task DeleteTransactions();
    void PrintIncome();
    void PrintExpenditures();
}