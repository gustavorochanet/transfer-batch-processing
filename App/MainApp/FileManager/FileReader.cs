namespace MainApp.FileManager;

/// <summary>
/// A concrete implementation of the <see cref="IFileReader"/> interface that reads files from the file system asynchronously.
/// </summary>
/// <remarks>
/// This class uses the <see cref="File.ReadAllLinesAsync(string)"/> method to read all lines from a specified file asynchronously.
/// It assumes that the file exists on the local file system and will handle typical file I/O exceptions.
/// </remarks>
public class FileReader : IFileReader
{
    /// <summary>
    /// Reads all lines from a specified file asynchronously.
    /// </summary>
    /// <param name="path">The path to the file to be read.</param>
    /// <returns>A task that represents the asynchronous operation, containing an array of strings where each string is a line from the file.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the specified file does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the caller does not have permission to access the file.</exception>
    /// <exception cref="IOException">Thrown if an I/O error occurs, such as a file being in use by another process.</exception>
    public async Task<string[]> ReadAllLinesAsync(string path)
    {
        // Using File.ReadAllLinesAsync to asynchronously read all lines of the file
        return await File.ReadAllLinesAsync(path);
    }
}
