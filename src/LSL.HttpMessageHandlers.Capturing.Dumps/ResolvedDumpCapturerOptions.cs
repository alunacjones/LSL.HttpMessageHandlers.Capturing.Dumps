using System;
using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class ResolvedDumpCapturerOptions(
    Func<RequestAndResponseDump, Task> handler,
    Func<Uri, Uri> uriTransformer,
    IHeaderMapper headerMapper) : IResolvedDumpCapturerOptions
{
    public Func<RequestAndResponseDump, Task> Handler => handler;
    public Func<Uri, Uri> UriTransformer => uriTransformer;
    public IHeaderMapper HeaderMapper => headerMapper;
}
