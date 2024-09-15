using MainApp.Entities;
using MainApp.FileManager;
using MainApp.Storage;
using MainApp.TransactionManager;
using Moq;

namespace TBP.Tests;

[TestClass]
public class TransactionProcessorTests
{
    private readonly Mock<IFileReader> _mockFileReader;
    private readonly Mock<ITransactionStorage> _mockStorage;
    private readonly TransactionProcessor _processor;

    public TransactionProcessorTests()
    {
        _mockFileReader = new Mock<IFileReader>();
        _mockStorage = new Mock<ITransactionStorage>();
        _processor = new TransactionProcessor(_mockStorage.Object);
    }

    [TestMethod]
    public void LoadTransactions_ShouldPopulateTransactionStorageCorrectly()
    {
        // Arrange
        var transactions = new List<GroupedTransaction>
            {
                new("1", 300.0), // Account 1 has two transactions: 100 + 200 = 300
                new("2", 300.0)  // Account 2 has one transaction: 300
            };

        _mockStorage.Setup(s => s.GetTransactions()).Returns([.. transactions]);

        // Act
        var loadedTransactions = _processor.CalculateCommissions();
        var commissions = _processor.CalculateCommissions();

        // Assert
        Assert.AreEqual(transactions.Count, loadedTransactions.Count);
        Assert.AreEqual(2, commissions.Count);
        Assert.IsTrue(commissions.ContainsKey("1"));
        Assert.IsTrue(commissions.ContainsKey("2"));
        Assert.AreEqual(30.0, commissions["2"]); // 300 * 0.1 = 30.0 for account 2
    }

    [TestMethod]
    public void RemoveHighestTransaction_ShouldRemoveOnlyHighestTransaction()
    {
        // Arrange
        var groupedTransactions = new List<GroupedTransaction>
        {
            new("1", 300.0), // Total for account 1 (100 + 200)
            new("2", 300.0)  // Total for account 2
        };


        _mockStorage.Setup(s => s.GetTransactions()).Returns([.. groupedTransactions]);
        _mockStorage.Setup(s => s.GetHigherTransaction())
                 .Returns(new Transaction("2", "TX3", 300.0)); // Highest transaction

        // Act
        var filteredTransactions = _processor.RemoveHighestTransaction();

        // Assert
        Assert.AreEqual(2, filteredTransactions.Length);
        Assert.AreEqual(0, filteredTransactions.First(t => t.AccountId == "2").TransactionSum, "Account 2 should have 0 after removing highest transaction.");
        Assert.AreEqual(300.0, filteredTransactions.First(t => t.AccountId == "1").TransactionSum, "Account 1 total should remain the same.");
    }

    [TestMethod]
    public void RemoveHighestTransaction_ShouldNotRemoveAndNotFailIfNoTransactions()
    {
        // Arrange
        _mockStorage.Setup(s => s.GetTransactions()).Returns([]);
        _mockStorage.Setup(s => s.GetHigherTransaction())
               .Returns(new Transaction(string.Empty, string.Empty, 0)); // Highest transaction

        // Act
        var filteredTransactions = _processor.RemoveHighestTransaction();
        var resultTransactions = _mockStorage.Object.GetTransactions();

        // Assert
        Assert.AreEqual(0, resultTransactions.Length);
        Assert.AreEqual(0, filteredTransactions.Length, "The collection should be empty.");
    }

    [TestMethod]
    public void CalculateCommissions_ShouldReturnCorrectCommissions()
    {
        // Arrange
        var transactions = new List<GroupedTransaction>
        {
            new("1", 250.0), // Total for account 1
            new("2", 300.0)  // Total for account 2
        };

        _mockStorage.Setup(s => s.GetTransactions()).Returns([.. transactions]);

        // Act
        var commissions = _processor.CalculateCommissions();

        // Assert
        Assert.AreEqual(2, commissions.Count);
        Assert.AreEqual(25.0, commissions["1"]); // (100 + 150) * 0.1
        Assert.AreEqual(30.0, commissions["2"]); // (300) * 0.1
    }

    [TestMethod]
    public void TransactionProcessor_ShouldHandleEmptyTransactions()
    {
        // Arrange
        _mockStorage.Setup(s => s.GetTransactions()).Returns([]);
        _mockStorage.Setup(s => s.GetHigherTransaction())
             .Returns(new Transaction(string.Empty, string.Empty, 0)); // Highest transaction


        var processor = new TransactionProcessor(_mockStorage.Object);

        // Act
        processor.RemoveHighestTransaction();
        var commissions = processor.CalculateCommissions();

        // Assert
        Assert.AreEqual(0, commissions.Count);
    }
}