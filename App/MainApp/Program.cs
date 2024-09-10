using System.CommandLine;
using MainApp.FileManager;
using MainApp.TransactionManager;

var fileOption = new Option<FileInfo?>(
            name: "filePath",
            description: "The path to the CSV file containing transaction data"){
                IsRequired = true, AllowMultipleArgumentsPerToken = false
            };



var fileArgument = new Argument<FileInfo?>(
    name: "file",  // This is just the name in code, it won't be required in the command line
    description: "The path to the CSV file containing transaction data"
) {
    Arity = ArgumentArity.ExactlyOne  // Expect exactly one argument
};


var rootCommand = new RootCommand("Transaction Processing Console App"){
    fileArgument
};
//rootCommand.AddOption(fileArgument);

rootCommand.SetHandler(async (file) =>
    {
        await ProcessFile(file!);
    }, fileOption);

return await rootCommand.InvokeAsync(args);

static async Task ProcessFile(FileInfo fileInfo)
{
    try
    {
        var fileReader = new FileReader();
        var processor = new TransactionProcessor(fileReader);

        var transactions = await processor.LoadTransactionsAsync(fileInfo.FullName);
        var filteredTransactions = processor.RemoveHighestTransaction(transactions);
        var commissions = processor.CalculateCommissions(filteredTransactions);
        PrintCommissions(commissions);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}


// Console.WriteLine("Hello, World!");

// var fileReader = new FileReader();
// var processor = new TransactionProcessor(fileReader);

// var transactions = await processor.LoadTransactionsAsync("SampleFiles/transactions_1.csv");
// var filteredTransactions = processor.RemoveHighestTransaction(transactions);
// var commissions = processor.CalculateCommissions(filteredTransactions);
// PrintCommissions(commissions);

static void PrintCommissions(Dictionary<string, double> commissions)
{
    foreach (var commission in commissions.OrderBy(x => x.Key))
    {
        Console.WriteLine($"{commission.Key},{commission.Value:F2}");
    }
}