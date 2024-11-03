using System.Globalization;
 public class Transactions
{
    public DateTime Date {get; set;}
    public string? TransactionType {get; set;}
    public decimal Amount {get; set;}
    public decimal Balance {get; set;}


    public override string ToString()
    {
        System.Console.WriteLine();
        return " > Date: " + Date + "\n > Type of transaction: " + TransactionType + "\n > Transfer amount: " + Amount;   
    }
}