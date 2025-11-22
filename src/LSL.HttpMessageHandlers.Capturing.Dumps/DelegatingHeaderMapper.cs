using System;
using System.Collections.Generic;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class DelegatingHeaderMapper(Func<IDictionary<string, IEnumerable<string>>, IDictionary<string, IEnumerable<string>>> @delegate) : IHeaderMapper
{
    public IDictionary<string, IEnumerable<string>> MapHeaders(IDictionary<string, IEnumerable<string>> originalHeaders) => 
        @delegate(originalHeaders);
}