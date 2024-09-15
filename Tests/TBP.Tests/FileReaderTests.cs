using MainApp.FileManager;

namespace TBP.Tests
{
    [TestClass]
    public class FileReaderTests
    {
        [TestMethod]
        public void FileReader_ShouldInvokeCallbackForEachValidLine()
        {
            // Arrange
            var tempFilePath = Path.GetTempFileName();
            File.WriteAllLines(tempFilePath, ["1,TX1,100.0", "1,TX2,200.0", "2,TX3,300.0"]);

            var fileReader = new FileReader(tempFilePath);
            var callbackInvokedCount = 0;

            // Act
            fileReader.ReadIntoCallback(t =>
            {
                callbackInvokedCount++;
                Assert.IsNotNull(t.accountId); // Basic validation to ensure callback received correct data
                Assert.IsNotNull(t.transactionId);
                Assert.IsTrue(t.transactionAmount > 0);
            });

            // Cleanup
            File.Delete(tempFilePath); // Delete the temporary file

            // Assert
            Assert.AreEqual(3, callbackInvokedCount); // Ensure callback is invoked exactly 3 times            
        }

    }
}
