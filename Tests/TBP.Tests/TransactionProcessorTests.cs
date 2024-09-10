using MainApp.Entities;
using MainApp.FileManager;
using MainApp.TransactionManager;
using Moq;

namespace TBP.Tests;

[TestClass]
public class TransactionProcessorTests
{
    private readonly Mock<IFileReader> _mockFileReader;
    private readonly TransactionProcessor _processor;

    public TransactionProcessorTests()
    {
        _mockFileReader = new Mock<IFileReader>();
        _processor = new TransactionProcessor(_mockFileReader.Object);
    }

    [TestMethod]
    public async Task LoadTransactionsAsync_ShouldLoadTransactionsCorrectly()
    {
        // Arrange
        var filePath = "sample.csv";
        var fileLines = new[] { "1,TX1,100.0", "1,TX2,200.0", "2,TX3,300.0" };
        _mockFileReader.Setup(fr => fr.ReadAllLinesAsync(filePath)).ReturnsAsync(fileLines);

        // Act
        var transactions = await _processor.LoadTransactionsAsync(filePath);

        // Assert
        Assert.AreEqual(3, transactions.Count);
        Assert.AreEqual("1", transactions[0].AccountId);
        Assert.AreEqual(200.0, transactions[1].TransactionAmount);
    }

    [TestMethod]
    public void RemoveHighestTransaction_ShouldRemoveOnlyHighestTransaction()
    {
        // Arrange
        var transactions = new List<Transaction>
            {
                new("1", "TX1", 100.0), // Use positional parameters to initialize the record
                new("1", "TX2", 200.0),
                new("2", "TX3", 300.0)
            };

        // Act
        var filteredTransactions = _processor.RemoveHighestTransaction(transactions);

        // Assert
        Assert.AreEqual(2, filteredTransactions.Count);
        Assert.IsFalse(filteredTransactions.Any(t => t.TransactionAmount == 300.0), "transaction with amount 300.0 was found"); // Remove the highest
        Assert.IsTrue(filteredTransactions.Any(t => t.TransactionAmount == 200.0)); // Lower transactions remain
    }

    [TestMethod]
    public void RemoveHighestTransaction_ShouldNotRemoveAndNotFailIfNoTransactions()
    {
        // Arrange
        var transactions = new List<Transaction>();

        // Act
        var filteredTransactions = _processor.RemoveHighestTransaction(transactions);

        // Assert
        Assert.AreEqual(0, filteredTransactions.Count, "The collection should be empty.");
    }

    [TestMethod]
    public void CalculateCommissions_ShouldReturnCorrectCommissions()
    {
        // Arrange
        var transactions = new List<Transaction>
            {
                new("1", "TX1", 100.0), // Use positional parameters to initialize the record
                new("1", "TX2", 150.0),
                new("2", "TX3", 300.0)
            };

        // Act
        var commissions = _processor.CalculateCommissions(transactions);

        // Assert
        Assert.AreEqual(2, commissions.Count);
        Assert.AreEqual(25.0, commissions["1"]); // (100 + 150) * 0.1
        Assert.AreEqual(30.0, commissions["2"]); // (300) * 0.1
    }
}