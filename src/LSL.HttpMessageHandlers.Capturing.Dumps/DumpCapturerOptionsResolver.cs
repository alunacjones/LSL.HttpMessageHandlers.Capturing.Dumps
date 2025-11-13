using System;
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
                .Build<Uri>(services => context => services.Aggregate(context, (agg, i) => i.Transform(agg))),
            compoundFactory
                .CreateBuilder([dumpCapturingOptions.HeaderMapperFactory])
                .Services
                .Single()
        );
}
