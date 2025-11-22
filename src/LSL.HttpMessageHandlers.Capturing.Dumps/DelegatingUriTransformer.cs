using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class DelegatingUriTransformer(Func<UriBuilder, UriBuilder> @delegate) : IUriTransformer
{
    public UriBuilder Transform(UriBuilder source) => @delegate(source);
}