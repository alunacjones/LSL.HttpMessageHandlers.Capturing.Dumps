using System.Collections.Generic;
using System.Linq;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal static class HeadersExtensions
{
    public static IDictionary<string, IEnumerable<string>> ToHeaderDictionary(this IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers) => 
        headers.GroupBy(h => h.Key).ToDictionary(h => h.Key, h => h.SelectMany(v => v.Value));
}