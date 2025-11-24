using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Internals;

internal class DelegatingUriTransformer(Func<UriBuilder, UriBuilder> @delegate) : IUriTransformer
{
    public UriBuilder Transform(UriBuilder source) => @delegate(source);
}