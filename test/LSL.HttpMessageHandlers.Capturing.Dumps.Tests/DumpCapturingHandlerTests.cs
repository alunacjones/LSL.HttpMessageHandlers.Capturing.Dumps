using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using LSL.ExecuteIf;
using LSL.HttpMessageHandlers.Capturing.Dumps.Tests.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class DumpCapturingHandlerTests
{
    [TestCase(false)]
    [TestCase(true)]
    public async Task WhenCreated_ItShouldDumpTheExpectedData(bool useCustomHeaderMapper)
    {
        // Arrange
        var capturedUrls = new List<string>();
        var provider = new ServiceCollection()
            .AddMockHttpMessageHandler()
            .Configure<TestOptions>(c =>
            {
                c.GetUri = "http://nowhere.com/?apikey=my-secret-thingy";
                c.Headers.Add(new("X-Remove", "remove it"));
                c.Headers.Add(new("X-Other-Remove", "duh"));
            })
            .AddHttpClient<MyTestClient>()            
            .AddRequestAndResponseCapturing(c => c
                .AddDumpCapturingHandler(c => c
                    .ExecuteIf(
                        useCustomHeaderMapper,
                        c => c.UseHeaderMapper<TestHeaderMapper>(),
                        c => c.UseDefaultHeaderMapper(c => 
                        { 
                            c.HeadersToObfuscate = ["X-Test"];
                            c.HeadersToRemove = ["X-Other-Remove"];
                        })
                    )
                    .AddQueryParameterObfuscatingUriTransformer()
                    .AddUriTransformer<TestUriTransformer>()
                    .AddDefaultDumpHandler()
                    .AddDumpHandlerDelegate(dump => capturedUrls.Add(dump.Request.RequestUri.ToString()))
                )
            )
            .Services
            .BuildServiceProvider();

        var options = provider.GetRequiredService<IOptions<TestOptions>>();
        var client = provider.GetRequiredService<MyTestClient>();
        var mockHttpMessageHandler = provider.GetMockHttpHandler();
        mockHttpMessageHandler.When(options.Value.GetUri)
            .Respond(
                new Dictionary<string, string>
                {
                    { "X-Test", "ewq" }
                },
                JsonContent.Create(new { Name = "Als"}));

        // Act
        await client.SendRequest();
        
        // Assert
        using var assertionScope = new AssertionScope();
        
        capturedUrls.Should().HaveCount(1);
    }

    private class MyTestClient
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

        public async Task SendRequest() => await _httpClient.GetAsync(_options.Value.GetUri);
    }

    private class TestOptions
    {
        public List<KeyValuePair<string, string>> Headers { get; set; } = [new("X-Test", "Als-secret-type-thing")];
        public string GetUri { get; set; } = "http://nowhere.com";
    }

    private class TestUriTransformer : IUriTransformer
    {
        public Uri Transform(Uri source)
        {
            return new UriBuilder(source).ConfigureWith(u =>
            {
                u.Path = "transformed-path";
            }).Uri;
        }
    }
    private class TestHeaderMapper : IHeaderMapper
    {
        public IDictionary<string, IEnumerable<string>> MapHeaders(IDictionary<string, IEnumerable<string>> originalHeaders)
        {
            if (originalHeaders.ContainsKey("X-Remove"))
            {
                originalHeaders.Remove("X-Remove");
            }

            return originalHeaders;
        }
    }
}