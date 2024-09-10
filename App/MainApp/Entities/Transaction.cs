namespace MainApp.Entities;

public class Transaction
{
    public required string AccountId { get; set; }
    public required string TransactionId { get; set; }
    public double TransactionAmount { get; set; }
}
