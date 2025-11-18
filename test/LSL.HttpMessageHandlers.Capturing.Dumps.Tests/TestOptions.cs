using System.Collections.Generic;
using System.Net.Http;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class TestOptions
{
    public List<KeyValuePair<string, string>> Headers { get; set; } = [new("X-Test", "Als-secret-type-thing")];
    public string Uri { get; set; } = "http://nowhere.com";
    public HttpMethod Method { get; set; } = HttpMethod.Post;
    public string RequestContentType = "application/json";
    public string RequestContent { get; set; } =
        """
        {
            "Id": "id1"
        }
        """;
}