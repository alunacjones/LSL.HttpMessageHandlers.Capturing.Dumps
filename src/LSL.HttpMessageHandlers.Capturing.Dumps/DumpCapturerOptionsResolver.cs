using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class DumpCapturerOptionsResolver(ICompoundFactory compoundFactory)
{
    public IResolvedDumpCapturerOptions Resolve(DumpCapturingOptions dumpCapturingOptions) =>
        new ResolvedDumpCapturerOptions(
            compoundFactory
                .CreateBuilder(dumpCapturingOptions.DumpHandlerFactories)
                .ForContext<RequestAndResponseDump>()
                .Build<Task>(handlers => async requestAndResponseDump =>
                {
                    foreach (var handler in handlers)
                    {
                        await handler.Dump(requestAndResponseDump).ConfigureAwait(false);
                    }
                }),
            compoundFactory
                .CreateBuilder(dumpCapturingOptions.UrlTransformersFactories)
                .ForContext<Uri>()
                .Build<Uri>(services => context => services.Aggregate(new UriBuilder(context), (agg, i) => i.Transform(agg)).Uri),
            compoundFactory
                .CreateBuilder(dumpCapturingOptions.HeaderMapperFactories)
                .ForContext<IDictionary<string, IEnumerable<string>>>()
                .Build<IDictionary<string, IEnumerable<string>>>(handlers => headers =>
                {
                    foreach (var handler in handlers)
                    {
                        handler.MapHeaders(headers);
                    }

                    return headers;
                })
        );
}
