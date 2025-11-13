using System;
using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Delegating dump handler options
/// </summary>
public class DelegatingDumpHandlerOptions
{
    /// <summary>
    /// A delegate to run on dumping a request and response
    /// </summary>
    public Func<RequestAndResponseDump, Task> DumpDelegate { get; set; }
}
