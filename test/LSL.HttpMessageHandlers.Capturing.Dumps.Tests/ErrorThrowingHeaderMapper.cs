using System;
using System.Collections.Generic;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class ErrorThrowingHeaderMapper : IHeaderMapper
{
    public IDictionary<string, IEnumerable<string>> MapHeaders(IDictionary<string, IEnumerable<string>> originalHeaders) => throw new NotImplementedException();
}
