using System;
using LSL.ExecuteIf;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class TestUriTransformer : IUriTransformer
{
    public UriBuilder Transform(UriBuilder source) => source.ConfigureWith(c => c.Path = "transformed-path");
}