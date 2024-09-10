namespace MainApp.FileManager
{
    public class FileReader : IFileReader
    {
        public async Task<string[]> ReadAllLinesAsync(string path)
        {
            return await File.ReadAllLinesAsync(path);
        }
    }
}