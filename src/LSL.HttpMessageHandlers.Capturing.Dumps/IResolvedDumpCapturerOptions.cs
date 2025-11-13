using System;
using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Internal resolved dump capturer options
/// </summary>
public interface IResolvedDumpCapturerOptions
{
    /// <summary>
    /// The composite dump handler
    /// </summary>
    Func<RequestAndResponseDump, Task> Handler { get; }

    /// <summary>
    /// The composite uri transformer
    /// </summary>
    Func<Uri, Uri> UriTransformer { get; }

    /// <summary>
    /// The header mapper
    /// </summary>
    IHeaderMapper HeaderMapper { get; }
}