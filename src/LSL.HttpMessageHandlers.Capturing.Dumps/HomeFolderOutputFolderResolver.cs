using System;
using System.IO;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class HomeFolderOutputFolderResolver(
    string name,
    IHomeFolderProvider homeFolderProvider,
    IOptionsMonitor<HomeFolderOutputFolderResolverOptions> optionsMonitor) : IOutputFolderResolver
{
    private readonly Lazy<HomeFolderOutputFolderResolverOptions> _options = new(() => optionsMonitor.Get(name));

    public string ResolveOutputFolder(RequestAndResponseDump requestAndResponseDump) => 
        Path.Combine(homeFolderProvider.GetHomeFolder(), ".http-dumps", _options.Value.SubFolder.ReplaceVariables(requestAndResponseDump));
}