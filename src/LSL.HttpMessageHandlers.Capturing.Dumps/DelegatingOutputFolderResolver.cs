using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class DelegatingOutputFolderResolver(Func<RequestAndResponseDump, string> outputFolderResolvingDelegate) : IOutputFolderResolver
{
    public string ResolveOutputFolder(RequestAndResponseDump requestAndResponseDump) => outputFolderResolvingDelegate(requestAndResponseDump);
}