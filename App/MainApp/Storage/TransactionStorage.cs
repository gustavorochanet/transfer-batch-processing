using MainApp.Entities;
using MainApp.FileManager;

namespace MainApp.Storage;

/// <summary>
/// An implementation of <see cref="ITransactionStorage"/> that stores transactions read from a file.
/// </summary>
/// <remarks>
/// This class uses an instance of <see cref="IFileReader"/> to read transactions from a file
/// and store them internally in a list. The stored transactions can then be retrieved by
/// other components, such as a transaction processor.
/// </remarks>
public class TransactionStorage : ITransactionStorage
{
    private readonly List<Transaction> _transactions = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionStorage"/> class and reads transactions from the file reader.
    /// </summary>
    /// <param name="fileReader">The <see cref="IFileReader"/> responsible for reading the transactions.</param>
    /// <remarks>
    /// This constructor immediately reads transactions using the provided <see cref="IFileReader"/>
    /// and stores them internally.
    /// </remarks>
    public TransactionStorage(IFileReader fileReader)
    {
        ReadTransactionsFromReader(fileReader);
    }

    /// <summary>
    /// Reads transactions from the file reader and stores them in the internal list.
    /// </summary>
    /// <param name="fileReader">The <see cref="IFileReader"/> that reads the transaction data.</param>
    private void ReadTransactionsFromReader(IFileReader fileReader)
    {
        fileReader.ReadIntoCallback( (t) => 
        {
            _transactions.Add(
                new Transaction(t.accountId, t.transactionId, t.transactionAmount)
            );
        });
    }

    /// <summary>
    /// Retrieves all stored transactions.
    /// </summary>
    /// <returns>An array of <see cref="Transaction"/> objects.</returns>
    public Transaction[] GetTransactions()
    {
        return [.. _transactions];
    }
}
