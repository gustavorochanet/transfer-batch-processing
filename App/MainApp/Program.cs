using MainApp.FileManager;
using MainApp.Storage;
using MainApp.TransactionManager;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

var app = new CommandApp<ProcessFileCommand>();
return app.Run(args);

internal class ProcessFileCommand : AsyncCommand<ProcessFileCommand.Settings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var filePath = settings.FilePath;

        // Ensure the file exists before processing
        if (!File.Exists(filePath))
        {
            AnsiConsole.Markup("[red]Error: The file does not exist.[/]");
            return -1;
        }

        // Process the file
        await ProcessFile(new FileInfo(filePath));
        return 0;
    }

    internal class Settings : CommandSettings
    {
        [CommandArgument(0, "<file>")]
        [Description("The path to the CSV file containing transaction data.")]
        public required string FilePath { get; set; }
    }

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
            // Create a progress bar to show progress across different stages
            await AnsiConsole.Status()
                .StartAsync("Processing transactions...", async ctx =>
                {
                    // Step 1: Reading the file
                    ctx.Status("[green]Reading file and loading transactions[/]");
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));

                    var fileReader = new FileReader(fileInfo.FullName);
                    var transactionStorage = new TransactionStorage(fileReader);

                    await Task.Delay(500); // Simulate delay for file reading (if required)

                    // Step 2: Storing transactions
                    ctx.Status("[yellow]Storing transactions...[/]");
                    ctx.Spinner(Spinner.Known.Clock);
                    ctx.SpinnerStyle(Style.Parse("yellow"));

                    await Task.Delay(500); // Simulate delay for storing transactions (if required)

                    // Step 3: Removing the highest transaction
                    ctx.Status("[blue]Removing highest transaction...[/]");
                    ctx.Spinner(Spinner.Known.Circle);
                    ctx.SpinnerStyle(Style.Parse("blue"));

                    var processor = new TransactionProcessor(transactionStorage);
                    processor.RemoveHighestTransaction();

                    await Task.Delay(500); // Simulate delay for removing transaction ( this one is likely to be needed)

                    // Step 4: Calculating commissions
                    ctx.Status("[magenta]Calculating commissions...[/]");
                    ctx.Spinner(Spinner.Known.Dots);
                    ctx.SpinnerStyle(Style.Parse("magenta"));

                    var commissions = processor.CalculateCommissions();

                    await Task.Delay(500); // Simulate delay for calculating commissions (this one is likely to be needed)

                    // Step 5: Print the results after all stages are done
                    ctx.Status("[green]Processing complete![/]");
                    PrintCommissions(commissions);

                    AnsiConsole.WriteLine($"{commissions.Count} commissions calculated. - {transactionStorage.TransactionCount} transactions read from file");
                });
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
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
        AnsiConsole.MarkupLine("[underline yellow]Commissions:[/]");

        foreach (var commission in commissions.OrderBy(x => x.Key))
        {
            // Print each account's commission formatted to 2 decimal places
            AnsiConsole.MarkupLine($"[green]{commission.Key}[/], [blue]{commission.Value:F2}[/]");
        }
    }
}


