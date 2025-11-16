using System.Collections.Generic;
using System.Collections.Specialized;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal static class NameValueCollectionExtensions
{
    public static NameValueCollection AddMultiple(this NameValueCollection source, string key, IEnumerable<string> values)
    {
        foreach (var value in values)
        {
            source.Add(key, value);
        }

        return source;
    }
}
