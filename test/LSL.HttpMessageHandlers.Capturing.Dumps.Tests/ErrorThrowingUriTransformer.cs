using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class ErrorThrowingUriTransformer : IUriTransformer
{
    public UriBuilder Transform(UriBuilder source) => throw new NotImplementedException();
}