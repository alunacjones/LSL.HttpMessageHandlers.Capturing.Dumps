using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class ResolvedDumpCapturerOptions(
    Func<RequestAndResponseDump, Task> handler,
    Func<Uri, Uri> uriTransformer,
    Func<IDictionary<string, IEnumerable<string>>, IDictionary<string, IEnumerable<string>>> headerMapper,
    IContentTypeBasedDeserialiser contentTypeBasedDeserialiser) : IResolvedDumpCapturerOptions
{
    public Func<RequestAndResponseDump, Task> Handler => handler;
    public Func<Uri, Uri> UriTransformer => uriTransformer;
    public Func<IDictionary<string, IEnumerable<string>>, IDictionary<string, IEnumerable<string>>> HeaderMapper => headerMapper;

    public IContentTypeBasedDeserialiser ContentTypeDeserialiser => contentTypeBasedDeserialiser;
}
