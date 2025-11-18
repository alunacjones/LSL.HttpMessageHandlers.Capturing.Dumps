using System;
using System.Linq;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class DefaultFilenameResolver(string name, IOptionsSnapshot<DefaultFilenameResolverOptions> optionsSnapshot) : IFilenameResolver
{
    private readonly Lazy<DefaultFilenameResolverOptions> _options = new(() => optionsSnapshot.Get(name));

    public string ResolveFilenameWithNoExtension(RequestAndResponseDump requestAndResponseDump)
    {
        var options = _options.Value;
        var pathSegments = requestAndResponseDump.Request.RequestUri.LocalPath.Split(Constants.PathCharacterArray, StringSplitOptions.RemoveEmptyEntries);

        var pathSegmentsToUse = options.TakePathSegments < 0
            ? pathSegments.Reverse().Skip(options.SkipPathSegments).Take(-options.TakePathSegments).Reverse()
            : pathSegments.Skip(options.SkipPathSegments).Take(options.TakePathSegments);

        return string.Join("_", [.. pathSegmentsToUse, Guid.NewGuid().ToString()])
            .MakeFilenameSafe();
    }    
}