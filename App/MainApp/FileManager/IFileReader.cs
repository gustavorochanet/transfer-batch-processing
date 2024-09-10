namespace MainApp.FileManager
{
    public interface IFileReader
    {
        Task<string[]> ReadAllLinesAsync(string path);
    }
}