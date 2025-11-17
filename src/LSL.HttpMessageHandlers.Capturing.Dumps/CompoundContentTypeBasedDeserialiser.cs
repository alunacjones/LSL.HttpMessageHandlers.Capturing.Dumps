using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class CompoundContentTypeBasedDeserialiser(IEnumerable<IContentTypeBasedDeserialiser> handlers) : IContentTypeBasedDeserialiser
{
    public async Task<JsonNode?> Deserialise(HttpContent httpContent)
    {
        if (httpContent is null) return null;

        foreach (var handler in handlers)
        {
            var result = await handler.Deserialise(httpContent).ConfigureAwait(false);

            if (result is not null) return result;
        }
        
        return null;
    }
}