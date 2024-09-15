using MainApp.Entities;
using MainApp.FileManager;
using MainApp.Storage;
using System.Globalization;

namespace MainApp.TransactionManager;

/// <summary>
/// Processes a list of transactions to handle business logic such as removing the highest transaction
/// and calculating commissions for accounts.
/// </summary>
/// <remarks>
/// This class operates on the transactions retrieved from <see cref="ITransactionStorage"/> and
/// provides functionality to manipulate and analyze the transactions.
/// The transaction list is lazily initialized from the <see cref="ITransactionStorage"/> to avoid
/// unnecessary data loading.
/// </remarks>
public class TransactionProcessor(ITransactionStorage transactionStorage)
{
    private readonly Lazy<List<Transaction>> _transactions = new(() => [.. transactionStorage.GetTransactions()]);

    /// <summary>
    /// Gets the list of transactions for processing.
    /// </summary>
    private List<Transaction> Transactions => _transactions.Value;

    /// <summary>
    /// Removes the transaction with the highest amount from the list of transactions.
    /// </summary>
    /// <remarks>
    /// This method modifies the internal list of transactions by removing only the highest
    /// transaction (if one exists).
    /// </remarks>
    public Transaction[] RemoveHighestTransaction()
    {
        var highestTransaction = Transactions.OrderByDescending(x => x.TransactionAmount).FirstOrDefault();
        if (highestTransaction != null)
        {
            Transactions.Remove(highestTransaction);
        }
        return [.. Transactions];
    }

    /// <summary>
    /// Calculates commissions for each account based on their transactions.
    /// </summary>
    /// <returns>A dictionary where the key is the account ID and the value is the calculated commission (10%).</returns>
    /// <remarks>
    /// The commission is calculated as 10% of the total transaction amount for each account.
    /// </remarks>
    public Dictionary<string, double> CalculateCommissions()
    {
        return Transactions
           .GroupBy(t => t.AccountId)
           .ToDictionary(
               group => group.Key,
               group => group.Sum(t => t.TransactionAmount) * 0.1 // Calculate 10% commission
           );
    }
}
