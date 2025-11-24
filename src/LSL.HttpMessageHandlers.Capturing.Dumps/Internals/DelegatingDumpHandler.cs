using System;
using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Internals;

/// <summary>
/// A delegating dump handler
/// </summary>
internal class DelegatingDumpHandler(Func<RequestAndResponseDump, Task> @delegate) : BaseDumpHandler
{
    /// <inheritdoc/>
    public override Task Dump(RequestAndResponseDump requestAndResponseDump) => @delegate(requestAndResponseDump);
}
