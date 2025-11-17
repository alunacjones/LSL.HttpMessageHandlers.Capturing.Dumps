using System.Collections.Generic;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// The contract for a header mapper
/// </summary>
public interface IHeaderMapper
{
    /// <summary>
    /// Maps headers from the original request or response
    /// </summary>
    /// <param name="originalHeaders"></param>
    /// <returns></returns>
    IDictionary<string, IEnumerable<string>> MapHeaders(IDictionary<string, IEnumerable<string>> originalHeaders);
}
