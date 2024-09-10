using MainApp.FileManager;
using MainApp.TransactionManager;
using System.CommandLine;

/// <summary>
/// Entry point for the Transaction Processing Console App.
/// The application accepts a file path as a command-line argument, processes the file,
/// and outputs transaction-related commissions.
/// </summary>
/// <remarks>
/// This example demonstrates the use of both the <see cref="FileInfo"/> parameter in the command line
/// handler and the usage of the <see cref="IFileReader"/> within the <see cref="TransactionProcessor"/>.
/// Even though we pass a <see cref="FileInfo"/> to the handler, the actual file reading is performed 
/// inside the <see cref="TransactionProcessor"/> to showcase both techniques.
/// </remarks>
var fileArgument = new Argument<FileInfo?>(
    name: "file",  // This is just the name in code, it won't be required in the command line
    description: "The path to the CSV file containing transaction data"
)
{
    Arity = ArgumentArity.ExactlyOne  // Expect exactly one argument
};

var rootCommand = new RootCommand("Transaction Processing Console App"){
    fileArgument
};

/// <summary>
/// Command handler that processes the file passed as an argument and triggers the transaction processing.
/// </summary>
/// <param name="file">The CSV file to be processed, provided as a <see cref="FileInfo"/>.</param>
/// <remarks>
/// The <see cref="FileInfo"/> parameter is used to exemplify the usage of file-based input in command-line
/// applications. Even though the file object is passed here, the actual content reading occurs inside 
/// the <see cref="TransactionProcessor"/> class.
/// </remarks>
rootCommand.SetHandler(async (file) =>
    {
        await ProcessFile(file!);
    }, fileArgument);

return await rootCommand.InvokeAsync(args);



/// <summary>
/// Asynchronously processes the file by loading transactions, removing the highest transaction, 
/// calculating commissions, and printing the results.
/// </summary>
/// <param name="fileInfo">The <see cref="FileInfo"/> object representing the CSV file.</param>
/// <remarks>
/// Although the file is passed as a <see cref="FileInfo"/>, this method delegates the actual reading 
/// of file contents to the <see cref="TransactionProcessor"/> to highlight the usage of both cases.
/// </remarks>
static async Task ProcessFile(FileInfo fileInfo)
{
    try
    {     
        // Creating the file reader and processor to handle transactions
        var fileReader = new FileReader();
        var processor = new TransactionProcessor(fileReader);

        // Loading transactions from the file using the processor
        var transactions = await processor.LoadTransactionsAsync(fileInfo.FullName);

        // Removing the highest transaction and calculating commissions
        var filteredTransactions = processor.RemoveHighestTransaction(transactions);
        var commissions = processor.CalculateCommissions(filteredTransactions);

        // Printing the final calculated commissions
        PrintCommissions(commissions);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

/// <summary>
/// Outputs the calculated commissions to the console.
/// </summary>
/// <param name="commissions">A dictionary containing the AccountId as the key and the calculated commission as the value.</param>
/// <remarks>
/// Commissions are printed with two decimal places, sorted by account ID for clarity.
/// </remarks>
static void PrintCommissions(Dictionary<string, double> commissions)
{
    foreach (var commission in commissions.OrderBy(x => x.Key))
    {
        // Print each account's commission formatted to 2 decimal places
        Console.WriteLine($"{commission.Key},{commission.Value:F2}");
    }
}