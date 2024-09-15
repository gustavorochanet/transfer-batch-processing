using MainApp.Entities;
using MainApp.FileManager;
using MainApp.Storage;
using Moq;

namespace TBP.Tests
{
    [TestClass]
    public class TransactionStorageTests
    {
        [TestMethod]
        public void TransactionStorage_ShouldStoreTransactionsCorrectly()
        {
            // Arrange
            var mockFileReader = new Mock<IFileReader>();
            var transactions = new List<Transaction>
            {
                new("1", "TX1", 100.0),
                new("1", "TX2", 200.0),
                new("2", "TX3", 300.0)
            };


            mockFileReader.Setup(fr => fr.ReadIntoCallback(It.IsAny<Action<Transaction>>()))
                .Callback<Action<Transaction>>(
                    callback =>
                    {
                        foreach (var t in transactions)
                        {
                            callback(t);
                        }
                    });

            // Act
            var storage = new TransactionStorage(mockFileReader.Object);
            var storedTransactions = storage.GetTransactions();

            // Assert
            Assert.AreEqual(2, storedTransactions.Length); // Two accounts are stored (grouped by account ID)
            Assert.AreEqual(300.0, storedTransactions[1].TransactionSum); // Total for second account
            Assert.AreEqual("1", storedTransactions[0].AccountId);
            Assert.AreEqual(300.0, storedTransactions[0].TransactionSum); // Total sum for account 1 (100 + 200)
        }

        [TestMethod]
        public void TransactionStorage_ShouldBeEmptyWhenNoTransactionsAreProvided()
        {
            // Arrange
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(fr => fr.ReadIntoCallback(It.IsAny<Action<Transaction>>()));

            var storage = new TransactionStorage(mockFileReader.Object);

            // Act
            var storedTransactions = storage.GetTransactions();

            // Assert
            Assert.AreEqual(0, storedTransactions.Length);
        }

    }
}
