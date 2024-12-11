using System.Globalization;

 public class Transaction
{
    public Guid transaction_Id {get; init;}
    public DateTime Date {get; set;}
    public string? TransactionType {get; set;}
    public decimal Amount {get; set;}
    //  public decimal Balance {get; set;}


    public override string ToString()
    {
        return $"\n > Date: " + Date + "\n > Type of transaction: " + TransactionType + "\n > Transfer amount: " + Amount ;   
    }
    // public string ToFilestring()
    // {
    //     return $"{Date.ToString("yyyy-MM-dd")}, {TransactionType}, {Amount}";
    // }
    

    //saving to file
    // public string toFilestring()
    // {
    //     return $"{Date: yyyy-MM-dd} , {TransactionType} , {Amount}";
    // }
}