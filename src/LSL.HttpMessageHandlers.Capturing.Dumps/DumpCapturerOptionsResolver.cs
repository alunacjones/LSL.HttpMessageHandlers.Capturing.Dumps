using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class DumpCapturerOptionsResolver(
    ILoggerFactory loggerFactory,
    ErrorLoggingExecutor errorLoggingExecutor,
    ICompoundFactory compoundFactory,
    ILogger<DumpCapturerOptionsResolver> logger)
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
                        await errorLoggingExecutor.ExecuteAsyncWithErrorHandling(
                            () => handler.Dump(requestAndResponseDump),
                            e => logger.LogError(e, "Exception thrown whilst executing dump handler of type {type}", handler.GetType())
                        ).ConfigureAwait(false);
                    }
                }),
            compoundFactory
                .CreateBuilder(dumpCapturingOptions.UrlTransformersFactories)
                .ForContext<Uri>()
                .Build<Uri>(services => context => services.Aggregate(
                    new UriBuilder(context), 
                    (agg, i) => errorLoggingExecutor.ExecuteWithErrorHandling(
                        () => i.Transform(agg), 
                        e => logger.LogError(e, "Exception thrown whilst executing url transformer of type {type}", i.GetType()),
                        agg)
                    )
                    .Uri),
            compoundFactory
                .CreateBuilder(dumpCapturingOptions.HeaderMapperFactories)
                .ForContext<IDictionary<string, IEnumerable<string>>>()
                .Build<IDictionary<string, IEnumerable<string>>>(handlers => headers =>
                {
                    foreach (var handler in handlers)
                    {
                        errorLoggingExecutor.ExecuteWithErrorHandling(
                            () => handler.MapHeaders(headers),
                            e => logger.LogError(e, "Exception thrown whilst executing header mapper of type {type}", handler)
                        );
                    }

                    return headers;
                }),
            new CompoundContentTypeBasedDeserialiser(
                loggerFactory.CreateLogger<CompoundContentTypeBasedDeserialiser>(),
                errorLoggingExecutor,
                compoundFactory
                    .CreateBuilder(dumpCapturingOptions.ContentTypeBasedDeserialiserFactories)
                    .Services
            )
        );
}
