using System.Collections.Generic;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// The contract for a header mapper
/// </summary>
public interface IHeaderMapper
{
    IDictionary<string, IEnumerable<string>> MapHeaders(IDictionary<string, IEnumerable<string>> originalHeaders);
}
