using System.Globalization;
using Spectre.Console;

if (args.Length < 1)
{
    Console.WriteLine("Please provide the desired file size in MB.");
    return;
}

if (!int.TryParse(args[0], out int fileSizeInMB))
{
    Console.WriteLine("Invalid file size provided. Please provide a valid integer.");
    return;
}

string filePath = "large_transactions.csv";
GenerateLargeTransactionFile(filePath, fileSizeInMB);
Console.WriteLine($"File '{filePath}' generated with an approximate size of {fileSizeInMB} MB.");


/// <summary>
/// Generates a large CSV file with transaction data.
/// </summary>
/// <param name="filePath">The path to save the generated file.</param>
/// <param name="fileSizeInMB">The desired file size in megabytes.</param>
static void GenerateLargeTransactionFile(string filePath, int fileSizeInMB)
{
    long targetFileSizeInBytes = fileSizeInMB * 1024L * 1024L; // Convert MB to Bytes
    long currentFileSize = 0;

    using var writer = new StreamWriter(filePath);
    Random random = new();
    int transactionCounter = 1;

    // Use Spectre.Console's Progress for tracking
    AnsiConsole.Progress()
        .Start(ctx =>
        {
            var task = ctx.AddTask("[green]Generating CSV file...[/]", maxValue: targetFileSizeInBytes);

            while (currentFileSize < targetFileSizeInBytes && !task.IsFinished)
            {
                string accountId = random.Next(1, 101).ToString();
                string transactionId = $"TX{transactionCounter:D7}";
                double transactionAmount = Math.Round(random.NextDouble() * 10000, 2);

                string line = $"{accountId},{transactionId},{transactionAmount.ToString(CultureInfo.InvariantCulture)}";
                writer.WriteLine(line);

                long lineSize = System.Text.Encoding.UTF8.GetByteCount(line + Environment.NewLine);
                currentFileSize += lineSize;

                // Increment progress based on the actual bytes written
                task.Increment(lineSize);
                transactionCounter++;
            }

            // Ensure task completes if it reaches 100%
            task.Value = task.MaxValue;
        });
}