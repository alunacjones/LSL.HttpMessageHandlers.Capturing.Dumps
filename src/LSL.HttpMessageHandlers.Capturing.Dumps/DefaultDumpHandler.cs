using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class DefaultDumpHandler(IOptionsSnapshot<DefaultDumpHandlerOptions> optionsSnapshot) : BaseDumpHandler
{
    private readonly Lazy<DefaultDumpHandlerOptions> _options = new(() => optionsSnapshot.Value);

    public override Task Dump(RequestAndResponseDump requestAndResponseDump)
    {
        // TODO
        var jsonSettings = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var toOutput = JsonSerializer.Serialize(requestAndResponseDump, jsonSettings);
        return Task.CompletedTask;
    }
}