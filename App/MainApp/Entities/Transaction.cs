namespace MainApp.Entities;

/// <summary>
/// Represents a financial transaction that includes an account identifier, 
/// a transaction identifier, and the amount involved in the transaction.
/// </summary>
/// <remarks>
/// This data type is defined as a record because:
/// 1. **Immutability**: Transactions are inherently immutable. Once a transaction is created, 
///    it should not be modified.
/// 2. **Value Equality**: Records provide value-based equality by default, which is ideal for 
///    comparing transactions. Two transactions with the same account ID, transaction ID, and amount 
///    are considered equal, even if they are different instances.
/// 3. **Conciseness**: Records offer a cleaner, more concise syntax for data containers, 
///    reducing boilerplate code that would otherwise be required in a class (e.g., constructors, 
///    equality checks, and `ToString` overrides).
/// </remarks>
public record Transaction(string AccountId, string TransactionId, double TransactionAmount);
