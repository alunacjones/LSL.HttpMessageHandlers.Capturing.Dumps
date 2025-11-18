using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using AngleSharp.Html;
using AngleSharp.Html.Parser;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class HtmlDeserialiser : IContentTypeBasedDeserialiser
{
    public async Task<JsonNode> Deserialise(HttpContent httpContent)
    {
        if (httpContent.Headers.ContentType.MediaType is not "text/html") return null;

        var parser = new HtmlParser();
        var stringContent = await httpContent.ReadAsStringAsync();
        var document = await parser.ParseDocumentAsync(stringContent);

        using var sw = new StringWriter();
        document.ToHtml(sw, new PrettyMarkupFormatter());

        return JsonValue.Create(sw.ToString().Split(['\n'], StringSplitOptions.None).Select(line => line.Replace("\t", "  ")));
    }
}