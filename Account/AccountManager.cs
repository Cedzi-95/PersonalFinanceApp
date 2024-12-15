public interface IaccountManager
{
    Transaction  CollectTransactionDetails();
   
    void CreateTransaction();
    void CheckBalance();
    void PrintAllTransactions();
    void DeleteTransactions();
    void PrintIncome();
    void PrintExpenditures();


}