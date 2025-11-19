using System.Reflection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Options for the home folder output folder resolver
/// </summary>
public class HomeFolderOutputFolderResolverOptions
{
    /// <summary>
    /// The sub folder under <c>{homeFolder}/.http-dumps</c> to store dumped requests and responses
    /// </summary>
public string SubFolder { get; set; } = Assembly.GetEntryAssembly().GetName().Name.MakeFilenameSafe();
}