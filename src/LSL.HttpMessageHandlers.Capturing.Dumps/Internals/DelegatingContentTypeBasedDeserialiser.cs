using System;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Internals;

internal class DelegatingContentTypeBasedDeserialiser(Func<HttpContent, Task<JsonNode?>> @delegate) : IContentTypeBasedDeserialiser
{
    public Task<JsonNode?> Deserialise(HttpContent httpContent) => @delegate(httpContent);
}