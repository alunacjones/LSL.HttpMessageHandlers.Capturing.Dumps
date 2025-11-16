using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Content and headers dump
/// </summary>
public class ContentAndHeadersDump
{
    /// <summary>
    /// The headers of the request or response
    /// </summary>
    public IDictionary<string, IEnumerable<string>> Headers { get; set; } = 
        new ReadOnlyDictionary<string, IEnumerable<string>>(new Dictionary<string, IEnumerable<string>>());

    /// <summary>
    /// The content of thew request or response
    /// </summary>
    public object Content { get; set; } = default!;    
}
