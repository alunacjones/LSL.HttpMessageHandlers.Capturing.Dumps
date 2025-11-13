using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class DefaultDumpHandler(IOptionsSnapshot<DefaultDumpHandlerOptions> optionsSnapshot) : BaseDumpHandler
{
    public override Task Dump(RequestAndResponseDump requestAndResponseDump)
    {
        // TODO
        var options = optionsSnapshot.Get(Name);
        return Task.CompletedTask;
    }
}