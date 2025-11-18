namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Output folder resolver interface
/// </summary>
public interface IOutputFolderResolver
{
    /// <summary>
    /// Resolves an output folder for the default dump handler
    /// </summary>
    /// <returns></returns>
    string ResolveOutputFolder(RequestAndResponseDump requestAndResponseDump);
}
