using MainApp.FileManager;
using MainApp.Storage;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var transactions = new List<(string accountId, string transactionId, double transactionAmount)>
            {
                ("1", "TX1", 100.0),
                ("1", "TX2", 200.0),
                ("2", "TX3", 300.0)
            };


            mockFileReader.Setup(fr => fr.ReadIntoCallback(It.IsAny<Action<(string, string, double)>>()))
                .Callback<Action<(string accountId, string transactionId, double transactionAmount)>>(
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
            Assert.AreEqual(transactions.Count, storedTransactions.Length);
            Assert.AreEqual("TX1", storedTransactions[0].TransactionId);
            Assert.AreEqual(300.0, storedTransactions[2].TransactionAmount);
        }

        [TestMethod]
        public void TransactionStorage_ShouldBeEmptyWhenNoTransactionsAreProvided()
        {
            // Arrange
            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(fr => fr.ReadIntoCallback(It.IsAny<Action<(string, string, double)>>()));

            var storage = new TransactionStorage(mockFileReader.Object);

            // Act
            var storedTransactions = storage.GetTransactions();

            // Assert
            Assert.AreEqual(0, storedTransactions.Length);
        }

    }
}
