using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Deserialises JSON content
/// </summary>
public class JsonDeserialiser : IContentTypeBasedDeserialiser
{
    /// <inheritdoc/>
    public async Task<JsonNode?> Deserialise(HttpContent httpContent) => 
        httpContent.Headers.ContentType.MediaType == "application/json"
            ? JsonNode.Parse(await httpContent.ReadAsStringAsync())
            : null;
}
