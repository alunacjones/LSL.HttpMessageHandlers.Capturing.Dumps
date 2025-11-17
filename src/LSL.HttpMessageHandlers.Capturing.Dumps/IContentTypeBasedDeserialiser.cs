using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Content type based deserialiser interface
/// </summary>
public interface IContentTypeBasedDeserialiser
{
    /// <summary>
    /// Attempts to deserialise a <see cref="HttpContent"/> instance
    /// </summary>
    /// <remarks>
    /// If <see langword="null"/> is returned then the next handler is run
    /// </remarks>
    /// <param name="httpContent"></param>
    /// <returns></returns>
    public Task<JsonNode?> Deserialise(HttpContent httpContent);
}