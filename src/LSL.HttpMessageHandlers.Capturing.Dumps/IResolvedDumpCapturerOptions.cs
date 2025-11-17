using System;
using System.Collections.Generic;
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
    Func<IDictionary<string, IEnumerable<string>>, IDictionary<string, IEnumerable<string>>> HeaderMapper { get; }

    /// <summary>
    /// THe content type based deserialiser
    /// </summary>
    IContentTypeBasedDeserialiser ContentTypeDeserialiser { get; }
}