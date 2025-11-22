using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class ErrorThrowingContentTypeDeserialiser : IContentTypeBasedDeserialiser
{
    public Task<JsonNode> Deserialise(HttpContent httpContent) => throw new System.NotImplementedException();
}