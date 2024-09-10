using MainApp.Entities;
using MainApp.FileManager;
using System.Globalization;

namespace MainApp.TransactionManager;

/// <summary>
/// Handles processing of transactions, including loading from a file, removing the highest transaction,
/// and calculating commissions.
/// </summary>
/// <remarks>
/// This class uses an implementation of <see cref="IFileReader"/> to abstract the file reading process, making it
/// easier to test and flexible for various file sources.
/// </remarks>
public class TransactionProcessor(IFileReader fileReader)
{
    private readonly IFileReader _fileReader = fileReader;

    /// <summary>
    /// Loads transactions from a CSV file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the CSV file containing transaction data.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of transactions.</returns>
    /// <exception cref="FormatException">Thrown when the CSV data is not in the expected format.</exception>
    /// <exception cref="IOException">Thrown when an I/O error occurs during reading the file.</exception>
    public async Task<List<Transaction>> LoadTransactionsAsync(string filePath)
    {
        var lines = await _fileReader.ReadAllLinesAsync(filePath);
        return lines.Select(line =>
        {
            var parts = line.Split(',');

            // Ensure the input has the expected format of three parts
            if (parts.Length != 3)
            {
                throw new FormatException("The CSV file is not in the correct format.");
            }

            // Parse and create a new Transaction record
            return new Transaction(
                parts[0], // AccountId
                parts[1], // TransactionId
                double.Parse(parts[2], CultureInfo.InvariantCulture) // TransactionAmount
            );

        }).ToList();
    }

    /// <summary>
    /// Removes the highest transaction amount from the provided list of transactions.
    /// </summary>
    /// <param name="transactions">The list of transactions to filter.</param>
    /// <returns>A list of transactions with the highest transaction removed.</returns>
    public List<Transaction> RemoveHighestTransaction(List<Transaction> transactions)
    {
        // Find and remove the single highest transaction from the list
        return
            transactions
                .OrderByDescending(x => x.TransactionAmount)
                .Skip(1) // Skip the highest transaction
                .ToList();
    }

    /// <summary>
    /// Calculates the commissions for each account based on the provided transactions.
    /// </summary>
    /// <param name="transactions">The list of transactions to process.</param>
    /// <returns>A dictionary where the key is the AccountId and the value is the calculated commission.</returns>
    /// <remarks>
    /// The commission is calculated as 10% of the total transaction amount for each account.
    /// </remarks>
    public Dictionary<string, double> CalculateCommissions(List<Transaction> transactions)
    {
        return transactions
           .GroupBy(t => t.AccountId)
           .ToDictionary(
               group => group.Key,
               group => group.Sum(t => t.TransactionAmount) * 0.1 // Calculate 10% commission
           );
    }
}