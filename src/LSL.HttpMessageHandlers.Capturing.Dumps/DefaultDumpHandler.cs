using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class DefaultDumpHandler : BaseDumpHandler
{
    private readonly Lazy<DefaultDumpHandlerOptions> _options;
    private readonly IHomeFolderProvider _homeFolderProvider;

    public DefaultDumpHandler(IOptionsSnapshot<DefaultDumpHandlerOptions> optionsSnapshot, IHomeFolderProvider homeFolderProvider)
    {
        _homeFolderProvider = homeFolderProvider;
        _options = new(() => optionsSnapshot.Get(Name));
    }

    public override Task Dump(RequestAndResponseDump requestAndResponseDump)
    {
        // TODO
        var homeFolder = _homeFolderProvider.GetHomeFolder();
        var options = _options.Value;
        var jsonSettings = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        var toOutput = JsonSerializer.Serialize(requestAndResponseDump, jsonSettings);
        File.WriteAllText(Path.Combine(homeFolder, $"{Guid.NewGuid()}.json"), toOutput);
        
        
        return Task.CompletedTask;
    }
}