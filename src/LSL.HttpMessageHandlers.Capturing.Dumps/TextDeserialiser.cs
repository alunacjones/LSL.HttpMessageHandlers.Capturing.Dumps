using System;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Deserialises text content
/// </summary>
public class TextDeserialiser : IContentTypeBasedDeserialiser
{
    /// <inheritdoc/>
    public async Task<JsonNode?> Deserialise(HttpContent httpContent) => 
        httpContent.Headers.ContentType.MediaType.StartsWith("text/")
            ? JsonValue.Create((await httpContent.ReadAsStringAsync()).Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries))
            : null;
}
