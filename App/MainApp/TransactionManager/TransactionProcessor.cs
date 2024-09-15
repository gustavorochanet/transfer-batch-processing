using MainApp.Entities;
using MainApp.Storage;

namespace MainApp.TransactionManager;

/// <summary>
/// Handles transaction processing logic such as removing the highest transaction 
/// and calculating commissions for accounts based on their transactions.
/// </summary>
/// <remarks>
/// This class interacts with an instance of <see cref="ITransactionStorage"/> to retrieve 
/// stored transactions and provides functionality for manipulating and analyzing the transactions.
/// The transaction list is lazily initialized from the <see cref="ITransactionStorage"/> 
/// to avoid unnecessary loading of all data until it is actually needed.
/// </remarks>
public class TransactionProcessor(ITransactionStorage transactionStorage)
{
    private readonly Lazy<List<GroupedTransaction>> _transactions = new(() => [.. transactionStorage.GetTransactions()]);
    private readonly ITransactionStorage _transactionStorage = transactionStorage;

    /// <summary>
    /// Gets the list of grouped transactions for processing.
    /// </summary>
    /// <remarks>
    /// This property lazily retrieves the list of transactions from the storage to avoid loading 
    /// the entire dataset into memory until it is actually needed.
    /// </remarks>
    private List<GroupedTransaction> Transactions => _transactions.Value;


    /// <summary>
    /// Removes the transaction with the highest amount from the list of transactions.
    /// </summary>
    /// <remarks>
    /// This method identifies the highest transaction from <see cref="ITransactionStorage.GetHigherTransaction"/>
    /// and reduces the transaction sum for the corresponding account. If a transaction exists for that account,
    /// the account's transaction total is reduced by the amount of the highest transaction.
    /// </remarks>
    /// <returns>An array of updated <see cref="GroupedTransaction"/> objects reflecting the removal of the highest transaction.</returns>
    public GroupedTransaction[] RemoveHighestTransaction()
    {
        var highestTransaction = _transactionStorage.GetHigherTransaction();
        if (!string.IsNullOrEmpty(highestTransaction.AccountId))
        {
            // Find the index of the transaction to replace
            var index = Array.FindIndex(Transactions.ToArray(), t => t.AccountId == highestTransaction.AccountId);

            if (index >= 0)
            {
                // Create a new record with the updated TransactionSum
                var updatedAccount = new GroupedTransaction(
                    Transactions[index].AccountId,
                    Transactions[index].TransactionSum - highestTransaction.TransactionAmount);

                // Replace the record in the array
                Transactions[index] = updatedAccount;
            }
        }

        return [.. Transactions];
    }

    /// <summary>
    /// Calculates commissions for each account based on their total transaction amounts.
    /// </summary>
    /// <remarks>
    /// The commission is calculated as 10% of the total transaction amount for each account. 
    /// This method processes the transactions and computes the commission for each account by 
    /// applying a 10% rate to the total amount of transactions associated with that account.
    /// </remarks>
    /// <returns>A dictionary where the key is the account ID and the value is the calculated commission (10%).</returns>
    public Dictionary<string, double> CalculateCommissions()
    {
        return Transactions
            .ToDictionary(
                t => t.AccountId,
                t => t.TransactionSum * 0.1
            );
    }
}
