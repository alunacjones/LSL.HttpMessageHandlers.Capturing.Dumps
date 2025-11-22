using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class CompoundContentTypeBasedDeserialiser(ILogger<CompoundContentTypeBasedDeserialiser> logger, ErrorLoggingExecutor errorLoggingExecutor, IEnumerable<IContentTypeBasedDeserialiser> handlers) : IContentTypeBasedDeserialiser
{
    public async Task<JsonNode?> Deserialise(HttpContent httpContent)
    {
        if (httpContent is null) return null;

        foreach (var handler in handlers)
        {
            var result = await errorLoggingExecutor.ExecuteAsyncWithErrorHandling(
                () => handler.Deserialise(httpContent),
                e => logger.LogError(e, "An exception was thrown when executing IContentTypeBasedDeserialiser of type {type}", handler.GetType()),
                null)
                .ConfigureAwait(false);

            if (result is not null) return result;
        }
        
        return null;
    }
}