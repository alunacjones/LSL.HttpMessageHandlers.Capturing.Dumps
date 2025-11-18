using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class MyTestClient
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<TestOptions> _options;

    public MyTestClient(HttpClient httpClient, IOptions<TestOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
        foreach (var header in options.Value.Headers)
        {
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
        }
    }

    public async Task SendRequest()
    {
        var options = _options.Value;
        var request = new HttpRequestMessage(options.Method, options.Uri);
        if (options.Method != HttpMethod.Get) request.Content = new StringContent(_options.Value.RequestContent, Encoding.UTF8, _options.Value.RequestContentType);
        await _httpClient.SendAsync(request);
    }
}