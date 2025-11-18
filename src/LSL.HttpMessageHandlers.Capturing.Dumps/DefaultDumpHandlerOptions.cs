using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Default dump handler options
/// </summary>
public class DefaultDumpHandlerOptions
{
    /// <summary>
    /// Json serializer options
    /// </summary>
    /// <remarks>
    /// <para>The initial settings are setup as follows:</para>
    /// <code>
    /// new JsonSerializerOptions
    /// {
    ///     PropertyNameCaseInsensitive = true,
    ///     PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    ///     WriteIndented = true,
    ///     Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    /// };
    /// </code>
    /// </remarks>
    public JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    internal ServiceProviderBasedFactory<IOutputFolderResolver> OutputFolderResolverFactory { get; set; } = 
        sp => ActivatorUtilities.CreateInstance<HomeFolderOutputFolderResolver>(sp, string.Empty);

    internal ServiceProviderBasedFactory<IFilenameResolver> FilenameResolverFactory { get; set; } =
        sp => ActivatorUtilities.CreateInstance<DefaultFilenameResolver>(sp, string.Empty);
}
