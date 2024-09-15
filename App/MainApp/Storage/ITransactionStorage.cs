using MainApp.Entities;

namespace MainApp.Storage;

/// <summary>
/// Defines a contract for storing and retrieving transactions, with support for multiple storage implementations.
/// </summary>
/// <remarks>
/// The idea behind this interface is to provide an abstraction for transaction storage, allowing for different
/// storage mechanisms such as in-memory storage (e.g., <see cref="TransactionStorage"/>) or more scalable 
/// storage solutions, like a NoSQL database.
/// 
/// Implementing this interface enables the decoupling of transaction storage from the rest of the application,
/// making it easier to switch between different storage backends depending on the application's needs.
/// 
/// For example:
/// - In-memory storage can be useful for testing or small datasets.
/// - A NoSQL database (e.g., MongoDB or CosmosDB) would be ideal for large datasets and distributed systems,
///   where scalability and persistence are required.
/// </remarks>
public interface ITransactionStorage
{
    /// <summary>
    /// Retrieves all stored transactions grouped by account ID.
    /// </summary>
    /// <returns>An array of <see cref="GroupedTransaction"/> objects, where each represents the total transaction amount per account.</returns>
    GroupedTransaction[] GetTransactions();

    /// <summary>
    /// Retrieves the transaction with the highest transaction amount.
    /// </summary>
    /// <returns>The <see cref="Transaction"/> object with the highest amount processed so far.</returns>
    Transaction GetHigherTransaction();

    /// <summary>
    /// Gets the total count of transactions that have been processed.
    /// </summary>
    /// <value>An integer representing the total number of transactions processed.</value>
    int TransactionCount { get; }
}
