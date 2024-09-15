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
        var transactions = new List<Transaction>
        {
            new("1", "TX1", 100.0),
            new("1", "TX2", 200.0),
            new("2", "TX3", 300.0)
        };

        _mockStorage.Setup(s => s.GetTransactions()).Returns([.. transactions]);

        // Act
        var loadedTransactions = _processor.CalculateCommissions();

        // Assert
        Assert.AreEqual(2, loadedTransactions.Count);
        Assert.IsTrue(loadedTransactions.ContainsKey("1"));
        Assert.IsTrue(loadedTransactions.ContainsKey("2"));
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

        _mockStorage.Setup(s => s.GetTransactions()).Returns([.. transactions]);

        // Act
        var filteredTransactions = _processor.RemoveHighestTransaction();

        // Assert
        Assert.AreEqual(2, filteredTransactions.Length);
        Assert.IsFalse(filteredTransactions.Any(t => t.TransactionAmount == 300.0));
        Assert.IsTrue(filteredTransactions.Any(t => t.TransactionAmount == 200.0)); // Lower transactions remain
    }

    [TestMethod]
    public void RemoveHighestTransaction_ShouldNotRemoveAndNotFailIfNoTransactions()
    {
        // Arrange
        var transactions = new List<Transaction>();

        _mockStorage.Setup(s => s.GetTransactions()).Returns([.. transactions]);

        // Act
        var filteredTransactions = _processor.RemoveHighestTransaction();

        // Assert
        var resultTransactions = _mockStorage.Object.GetTransactions();
        Assert.AreEqual(0, resultTransactions.Length);
        Assert.AreEqual(0, filteredTransactions.Length, "The collection should be empty.");
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
        var mockStorage = new Mock<ITransactionStorage>();
        mockStorage.Setup(s => s.GetTransactions()).Returns([]);

        var processor = new TransactionProcessor(mockStorage.Object);

        // Act
        processor.RemoveHighestTransaction();
        var commissions = processor.CalculateCommissions();

        // Assert
        Assert.AreEqual(0, commissions.Count);
    }   
}