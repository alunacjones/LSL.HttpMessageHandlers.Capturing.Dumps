using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// A deserialiser that returns a string <see cref="JsonNode"/> with the content <c>Redacted</c>
/// </summary>
/// <remarks>
/// If this is used it is advised to add it last.
/// </remarks>
public class RedactingDeserialiser : IContentTypeBasedDeserialiser
{
    /// <inheritdoc/>
    public Task<JsonNode?> Deserialise(HttpContent httpContent)
    {
        JsonNode? result = "Redacted";
        return Task.FromResult(result);
    }
}