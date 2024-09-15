using MainApp.Entities;

namespace MainApp.Storage;

/// <summary>
/// Defines a contract for storing and retrieving transactions.
/// </summary>
/// <remarks>
/// This interface abstracts the transaction storage, allowing for different storage mechanisms.
/// It is designed to decouple the storage from the file reading and transaction processing logic.
/// </remarks>
public interface ITransactionStorage
{
    /// <summary>
    /// Retrieves all stored transactions.
    /// </summary>
    /// <returns>An array of <see cref="Transaction"/> objects.</returns>
    Transaction[] GetTransactions();
}
