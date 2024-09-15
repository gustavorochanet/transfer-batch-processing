using MainApp.Entities;
using MainApp.FileManager;

namespace MainApp.Storage;

/// <summary>
/// An implementation of <see cref="ITransactionStorage"/> that stores transactions read from a file.
/// </summary>
/// <remarks>
/// This class reads transactions using an instance of <see cref="IFileReader"/> and stores them internally.
/// It provides methods to retrieve the total number of transactions, transactions grouped by account,
/// and the transaction with the highest amount.
/// </remarks>
public class TransactionStorage : ITransactionStorage
{
    private readonly Dictionary<string, double> _transactions = []; // Stores the total transaction amount per account
    private Transaction _higherTransaction = new(string.Empty, string.Empty, 0); // Stores the highest transaction
    private int transactionCount = 0; // Tracks the total number of transactions

    /// <summary>
    /// Gets the total number of transactions processed so far.
    /// </summary>
    public int TransactionCount => transactionCount;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionStorage"/> class and reads transactions from the file reader.
    /// </summary>
    /// <param name="fileReader">The <see cref="IFileReader"/> responsible for reading the transactions from the file.</param>
    /// <remarks>
    /// The constructor reads the transactions using the provided file reader and immediately stores them in memory.
    /// </remarks>
    public TransactionStorage(IFileReader fileReader)
    {
        ReadTransactionsFromReader(fileReader);
    }

    /// <summary>
    /// Reads transactions from the file reader and updates the internal transaction storage.
    /// </summary>
    /// <param name="fileReader">The <see cref="IFileReader"/> that reads the transaction data.</param>
    private void ReadTransactionsFromReader(IFileReader fileReader)
    {
        fileReader.ReadIntoCallback( (t) => 
        {
            transactionCount++;
            UpdateTransactions(t);
            UpdateHigherTransaction(t);
        });
    }

    /// <summary>
    /// Updates the highest transaction with the new transaction if it has a higher amount.
    /// </summary>
    /// <param name="transaction">The new transaction to be checked against the current highest transaction.</param>

    private void UpdateHigherTransaction(Transaction transaction)
    {
        if (transaction.TransactionAmount > _higherTransaction.TransactionAmount)
        {
            _higherTransaction = transaction;
        }
    }

    /// <summary>
    /// Updates the internal transaction storage by adding the new transaction's amount to the appropriate account.
    /// </summary>
    /// <param name="transaction">The new transaction to be added to the storage.</param>

    private void UpdateTransactions(Transaction transaction)
    {
        if (!_transactions.ContainsKey(transaction.AccountId))
        {
            _transactions[transaction.AccountId] = transaction.TransactionAmount;
        }
        else
        {
            _transactions[transaction.AccountId] += transaction.TransactionAmount;
        }
    }

    /// <summary>
    /// Retrieves all stored transactions grouped by account.
    /// </summary>
    /// <returns>An array of <see cref="GroupedTransaction"/> objects where each represents the total transaction amount per account.</returns>
    public GroupedTransaction[] GetTransactions()
    {
        return _transactions.Select(t => new GroupedTransaction(t.Key, t.Value)).ToArray();
    }

    /// <summary>
    /// Retrieves the transaction with the highest amount.
    /// </summary>
    /// <returns>The <see cref="Transaction"/> object with the highest transaction amount.</returns>
    public Transaction GetHigherTransaction()
    {
        return _higherTransaction;
    }
}
