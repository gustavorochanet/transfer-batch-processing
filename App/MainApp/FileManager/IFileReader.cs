using MainApp.Entities;

namespace MainApp.FileManager;

/// <summary>
/// Defines a contract for reading transaction data from various sources.
/// </summary>
/// <remarks>
/// This interface abstracts the file reading operation, allowing for different implementations
/// to read data from files or other sources (e.g., memory, remote locations). The data
/// is passed through a callback function, enabling flexible processing and testing.
/// </remarks>
public interface IFileReader
{
    /// <summary>
    /// Reads transaction data and invokes the provided callback for each transaction.
    /// </summary>
    /// <param name="callbackFunction">The callback function to be invoked for each transaction line.</param>
    /// <remarks>
    /// The implementation of this method is responsible for reading from a specific source (e.g., a file)
    /// and parsing each line into a valid transaction format before invoking the callback.
    /// </remarks>
    public void ReadIntoCallback(Action<Transaction> callbackFunction);
}
