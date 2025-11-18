using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class DelegatingFilenameResolver(Func<RequestAndResponseDump, string> provider) : IFilenameResolver
{
    public string ResolveFilenameWithNoExtension(RequestAndResponseDump requestAndResponseDump) => provider(requestAndResponseDump);
}