using MainApp.Entities;
using MainApp.FileManager;

namespace MainApp.TransactionManager;

public class TransactionProcessor(IFileReader fileReader)
{
    private readonly IFileReader _fileReader = fileReader;


    public async Task<List<Transaction>> LoadTransactionsAsync(string filePath)
    {
        var lines = await _fileReader.ReadAllLinesAsync(filePath);
        return lines.Select(line =>
        {
            var parts = line.Split(',');
            return new Transaction
            {
                AccountId = parts[0],
                TransactionId = parts[1],
                TransactionAmount = double.Parse(parts[2])
            };
        }).ToList();
    }

    public List<Transaction> RemoveHighestTransaction(List<Transaction> transactions)
    {
        return
            transactions
                .OrderByDescending(x => x.TransactionAmount)
                .Skip(1)
                .ToList();
    }

    public Dictionary<string, double> CalculateCommissions(List<Transaction> transactions)
    {
        var commissions = transactions.GroupBy(t => t.AccountId)
            .ToDictionary(
                group => group.Key,
                group => group.Sum(t => t.TransactionAmount) * 0.1
            );
        return commissions;
    }
}