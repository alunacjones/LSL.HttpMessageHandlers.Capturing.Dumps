namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Directory creation abstraction
/// </summary>
public interface IDirectoryCreator
{
    /// <summary>
    /// Creates the directory (and any subfolders) and returns the full path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string CreateDirectory(string path);
}
