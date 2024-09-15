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

            try
            {
                fileReader.ReadIntoCallback(t =>
                {
                    callbackInvokedCount++;

                    // Validate that the callback was invoked with correct transaction details
                    Assert.IsFalse(string.IsNullOrEmpty(t.AccountId), "Account ID should not be null or empty.");
                    Assert.IsFalse(string.IsNullOrEmpty(t.TransactionId), "Transaction ID should not be null or empty.");
                    Assert.IsTrue(t.TransactionAmount > 0, "Transaction amount should be greater than 0.");
                });
            }
            finally
            {
                // Cleanup: Ensure the temporary file is deleted after the test
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }


            // Assert
            Assert.AreEqual(3, callbackInvokedCount); // Ensure callback is invoked exactly 3 times            
        }

    }
}
