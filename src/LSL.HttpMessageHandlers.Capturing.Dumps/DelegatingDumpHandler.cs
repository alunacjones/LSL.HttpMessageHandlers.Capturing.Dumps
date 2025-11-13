using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// A delegating dump handler
/// </summary>
internal class DelegatingDumpHandler : BaseDumpHandler
{
    private readonly IOptionsSnapshot<DelegatingDumpHandlerOptions> _optionsSnapshot;
    private readonly Lazy<DelegatingDumpHandlerOptions> _options;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="optionsSnapshot"></param>
    public DelegatingDumpHandler(IOptionsSnapshot<DelegatingDumpHandlerOptions> optionsSnapshot)
    {
        _optionsSnapshot = optionsSnapshot;
        _options = new(() => _optionsSnapshot.Get(Name));
    }

    /// <inheritdoc/>
    public override Task Dump(RequestAndResponseDump requestAndResponseDump) => _options.Value.DumpDelegate(requestAndResponseDump);
}
