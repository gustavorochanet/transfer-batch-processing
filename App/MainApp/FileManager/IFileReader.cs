namespace MainApp.FileManager;

/// <summary>
/// Defines a contract for reading files asynchronously.
/// </summary>
/// <remarks>
/// This interface abstracts the file reading operation, allowing for different implementations
/// that can read from various sources (e.g., the file system, memory, or a remote location).
/// It enables easier unit testing and decouples the file reading logic from the core application logic.
/// </remarks>
public interface IFileReader
{
    /// <summary>
    /// Reads all lines from a specified file asynchronously.
    /// </summary>
    /// <param name="path">The path to the file to be read.</param>
    /// <returns>A task that represents the asynchronous operation, containing an array of strings, where each string is a line from the file.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the specified file does not exist.</exception>
    /// <exception cref="IOException">Thrown when an I/O error occurs during reading.</exception>
    Task<string[]> ReadAllLinesAsync(string path);
}
