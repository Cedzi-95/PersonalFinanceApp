using System.Globalization;

 public class Transaction
{
    public Guid TransactionId {get; init;}
    public User? User {get; init;}
    public DateTime Date {get; init;}
    public string? TransactionType {get; set;}
    public decimal Amount {get; set;}
    //  public decimal Balance {get; set;}


    public override string ToString()
    {
        return $"\n > Date: " + Date + "\n > Type of transaction: " + TransactionType + "\n > Transfer amount: " + Amount ;   
    }
}