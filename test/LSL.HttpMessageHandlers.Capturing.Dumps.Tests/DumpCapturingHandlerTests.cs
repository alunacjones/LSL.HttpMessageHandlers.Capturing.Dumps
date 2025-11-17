using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Diamond.Core.System.TemporaryFolder;
using FluentAssertions;
using FluentAssertions.Execution;
using LSL.ExecuteIf;
using LSL.HttpMessageHandlers.Capturing.Core;
using LSL.HttpMessageHandlers.Capturing.Dumps.Tests.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class DumpCapturingHandlerTests
{
    [TestCase(false)]
    [TestCase(true)]
    [TestCase(true, "text/plain", "Hello there")]
    [TestCase(true, "application/other", "Hello there")]
    public async Task WhenCreated_ItShouldDumpTheExpectedData(bool useCustomHeaderMapper, string requestContentType = null, string requestContent = null)
    {
        // Arrange
        using var tempFolder = new TemporaryFolderFactory().Create();
        
        var capturedUrls = new List<string>();
        var provider = new ServiceCollection()
            .AddSingleton<IHomeFolderProvider>(sp => new TestHomeFolderProvider(tempFolder.FullPath))       
            .AddMockHttpMessageHandler()
            .Configure<TestOptions>(c =>
            {
                c.PostUri = "http://nowhere.com/?apikey=my-secret-thingy&api_key=other-one";
                c.Headers.AddRange([
                    new("X-Remove", "remove it"),
                    new("X-Other-Remove", "duh"),
                    new("Authorization", "Bearer my-fully-secret-thingy")              
                ]);

                if (requestContentType is not null) c.RequestContentType = requestContentType;
                if (requestContent is not null) c.RequestContent = requestContent;
            })
            .AddHttpClient<MyTestClient>()            
            .AddRequestAndResponseCapturing(c => c
                .AddDumpCapturingHandler(c => c
                    .AddDefaultContentTypeBasedDeserialisers()
                    .ExecuteIf(
                        useCustomHeaderMapper,
                        c => c.AddHeaderMapper<TestHeaderMapper>(),
                        c => c.AddDefaultHeaderMapper(c => c
                            .UseDefaultObfuscator(c => 
                            { 
                                c.NumberOfCharactersToKeepClear = 3;
                                c.ObfuscatingSuffix = "!!!";
                            }).HeadersToRemove = ["X-Other-Remove"]
                        )
                    )
                    .AddQueryParameterObfuscatingUriTransformer(c => c.UseDefaultObfuscator(c => c.ObfuscatingSuffix = "**"))
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
        mockHttpMessageHandler.When(options.Value.PostUri)
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

    [Test]
    public async Task WhenCreated_ItShouldDumpTheExpectedData()
    {
        // Arrange
        using var tempFolder = new TemporaryFolderFactory().Create();

        var capturedUrls = new List<string>();
        var provider = new ServiceCollection()
            .AddMockHttpMessageHandler()
            .Configure<TestOptions>(c =>
            {
                c.PostUri = "http://nowhere.com/?apikey=my-secret-thingy";
                c.Headers.AddRange([
                    new("X-Remove", "remove it"),
                    new("X-Other-Remove", "duh"),
                    new("Authorization", "Bearer my-fully-secret-thingy")
                ]);
            })
            .AddHttpClient<MyTestClient>()            
            .AddRequestAndResponseCapturing(c => c
                .AddDumpCapturingHandlerWithDefaults(c => c
                    .AddDumpHandlerDelegate(dump => capturedUrls.Add(dump.Request.RequestUri.ToString()))
                )
            )
            .Services
            .BuildServiceProvider();

        var options = provider.GetRequiredService<IOptions<TestOptions>>();
        var client = provider.GetRequiredService<MyTestClient>();
        var mockHttpMessageHandler = provider.GetMockHttpHandler();
        mockHttpMessageHandler.When(options.Value.PostUri)
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

    [Test]
    public void GivenANullUriTransformer_ItShouldThrowAnArgumentNullException()
    {
        // Arrange
        var providerAction =  new Action(() => new ServiceCollection()
            .Configure<TestOptions>(c =>
            {
                c.PostUri = "http://nowhere.com/?apikey=my";
            })
            .AddHttpClient<MyTestClient>()
            .AddRequestAndResponseCapturing(c => c
                .AddDumpCapturingHandler(c => c.AddUriTransformer(null))
            )
            .Services
            .BuildServiceProvider());

        // Act & Assert
        providerAction.Should().ThrowExactly<ArgumentNullException>();
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

        public async Task SendRequest() => await _httpClient.PostAsync(_options.Value.PostUri, new StringContent(_options.Value.RequestContent, Encoding.UTF8, _options.Value.RequestContentType));
    }

    private class TestOptions
    {
        public List<KeyValuePair<string, string>> Headers { get; set; } = [new("X-Test", "Als-secret-type-thing")];
        public string PostUri { get; set; } = "http://nowhere.com";
        public string RequestContentType = "application/json";
        public string RequestContent { get; set; } = 
            """
            {
                "Id": "id1"
            }
            """;
    }

    private class TestUriTransformer : IUriTransformer
    {
        public UriBuilder Transform(UriBuilder source) => source.ConfigureWith(c => c.Path = "transformed-path");
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

    private class TestHomeFolderProvider(string folderPath) : IHomeFolderProvider
    {
        public string GetHomeFolder() => folderPath;
    }
}