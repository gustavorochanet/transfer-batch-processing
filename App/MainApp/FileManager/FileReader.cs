using MainApp.Entities;

namespace MainApp.FileManager;

/// <summary>
/// A concrete implementation of the <see cref="IFileReader"/> interface that reads transactions from a file.
/// </summary>
/// <remarks>
/// This class reads transaction data from a file in CSV format and passes the parsed data to a callback function.
/// The file must exist on the local filesystem, and each line in the file should represent a single transaction
/// in the format: accountId, transactionId, transactionAmount.
/// </remarks>
public class FileReader : IFileReader
{
    private readonly string _filePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileReader"/> class.
    /// </summary>
    /// <param name="filePath">The path to the transaction file.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="filePath"/> is null.</exception>
    /// <exception cref="FileNotFoundException">Thrown if the specified file does not exist.</exception>
    public FileReader(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath);

        if (!File.Exists(filePath)) 
        { 
            
            throw new FileNotFoundException("Transaction File does not exist", filePath);
        }

        _filePath = filePath;
    }

    /// <summary>
    /// Reads the file line by line and invokes the provided callback for each valid transaction line.
    /// </summary>
    /// <param name="callbackFunction">The callback to process each valid transaction line.</param>
    /// <remarks>
    /// Each line of the file is expected to contain a transaction in the format: accountId, transactionId, transactionAmount.
    /// Invalid or malformed lines are skipped.
    /// </remarks>
    public void ReadIntoCallbackOld(Action<(string accountId, string transactionId, double transactionAmount)> callbackFunction)
    {
        using var fileStream = File.OpenRead(_filePath);
        using var streamReader = new StreamReader(fileStream);

        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parts = line.Split(',');
            if (parts.Length != 3)
            {
                continue;
            }

            var accountId = parts[0];
            var transactionId = parts[1];
            if (!double.TryParse(parts[2], out var transactionAmount))
            {
                continue;
            }

            callbackFunction((accountId, transactionId, transactionAmount));
        }
    }

    /// <summary>
    /// Reads the file line by line and invokes the provided callback for each valid transaction line.
    /// </summary>
    /// <param name="callbackFunction">The callback to process each valid transaction line.</param>
    /// <remarks>
    /// Each line of the file is expected to contain a transaction in the format: accountId, transactionId, transactionAmount.
    /// Invalid or malformed lines are skipped.
    /// </remarks>
    public void ReadIntoCallback(Action<Transaction> callbackFunction)
    {
        using var fileStream = File.OpenRead(_filePath);
        using var streamReader = new StreamReader(fileStream);

        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var span = line.AsSpan(); // Avoid creating string arrays with Split
            var index1 = span.IndexOf(',');
            if (index1 == -1) continue; // Invalid format, skip line

            var index2 = span[(index1 + 1)..].IndexOf(',') + index1 + 1;
            if (index2 == -1) continue; // Invalid format, skip line

   
            var accountId = span[..index1].ToString();
            var transactionId = span.Slice(index1 + 1, index2 - index1 - 1).ToString();

            if (!double.TryParse(span[(index2 + 1)..], out var transactionAmount))
            {
                continue; // Skip line if parsing fails
            }

            callbackFunction(new Transaction(accountId, transactionId, transactionAmount));
        }
    }
}
