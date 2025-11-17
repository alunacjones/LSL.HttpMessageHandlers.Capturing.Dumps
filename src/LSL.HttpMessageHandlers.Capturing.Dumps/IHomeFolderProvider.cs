namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Home folder provider interface
/// </summary>
public interface IHomeFolderProvider
{
    /// <summary>
    /// Gets the current user's home folder 
    /// </summary>
    /// <returns></returns>
    string GetHomeFolder();
}
