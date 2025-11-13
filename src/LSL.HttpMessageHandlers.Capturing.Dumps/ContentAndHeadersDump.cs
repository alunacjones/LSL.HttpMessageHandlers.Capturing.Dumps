using System.Collections.Generic;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Content and headers dump
/// </summary>
public class ContentAndHeadersDump
{
    /// <summary>
    /// The headers of the request or response
    /// </summary>
    public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers { get; set; } = [];

    /// <summary>
    /// The content of thew request or response
    /// </summary>
    public object Content { get; set; } = default!;    
}
