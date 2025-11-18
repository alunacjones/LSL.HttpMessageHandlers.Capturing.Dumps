using System.Collections.Generic;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class TestHeaderMapper : IHeaderMapper
{
    public IDictionary<string, IEnumerable<string>> MapHeaders(IDictionary<string, IEnumerable<string>> originalHeaders)
    {
        if (originalHeaders.ContainsKey("X-Remove"))
        {
            originalHeaders.Remove("X-Remove");
        }

        return originalHeaders;
    }
}