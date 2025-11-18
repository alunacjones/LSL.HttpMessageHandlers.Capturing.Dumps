namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// IFilenameResolver
/// </summary>
public interface IFilenameResolver
{
    /// <summary>
    /// Resolves a base filename with no extension
    /// </summary>
    /// <param name="requestAndResponseDump"></param>
    /// <returns></returns>
    string ResolveFilenameWithNoExtension(RequestAndResponseDump requestAndResponseDump);
}
