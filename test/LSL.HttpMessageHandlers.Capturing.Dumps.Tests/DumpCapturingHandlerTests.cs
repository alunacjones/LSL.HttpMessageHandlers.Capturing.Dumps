using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Diamond.Core.System.TemporaryFolder;
using FluentAssertions;
using FluentAssertions.Execution;
using LSL.Disposables.CurrentDirectory;
using LSL.ExecuteIf;
using LSL.HttpMessageHandlers.Capturing.Core;
using LSL.HttpMessageHandlers.Capturing.Dumps.Tests.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class DumpCapturingHandlerTests
{
    [TestCase(false)]
    [TestCase(true)]
    [TestCase(
        true, 
        "text/plain", 
        "Hello there\r\nAnd there's more!")]
    [TestCase(
        true,
        "text/html",
        "<html><head></head><body><i></i></body></html>")]
    [TestCase(
        true,
        "application/other",
        "Hello there")]
    [TestCase(
        true, "application/other", "Hello there", 
        ResolutionOptions.Custom)]
    [TestCase(
        true,
        "application/other",
        "Hello there",
        ResolutionOptions.Delegate)]
    [TestCase(
        true,
        "application/other",
        "Hello there",
        ResolutionOptions.Delegate, 
        ResolutionOptions.Custom)]
    [TestCase(
        true,
        "application/other",
        "Hello there",        
        ResolutionOptions.Delegate, 
        ResolutionOptions.Delegate)]
    [TestCase(
        true, 
        "text/other", 
        "Hello there\r\nother\nstuff", 
        ResolutionOptions.Delegate, 
        ResolutionOptions.Delegate, 
        DefaultContentTypeBasedDeserialisersExclusions.RedactingDeserialiser)]
    [TestCase(
        true, 
        "application/other", 
        "Hello there\r\nother\nstuff", 
        ResolutionOptions.Delegate, 
        ResolutionOptions.Delegate, 
        DefaultContentTypeBasedDeserialisersExclusions.RedactingDeserialiser, 
        TestHttpMethod.Get)]
    [TestCase(
        true, 
        "text/other", 
        "Hello there\r\nother\nstuff")]    
    public async Task WhenCreated_ItShouldDumpTheExpectedData(
        bool useCustomHeaderMapper,
        string requestContentType = null,
        string requestContent = null,
        ResolutionOptions outputFolderResolution = ResolutionOptions.Default,
        ResolutionOptions filenameResolution = ResolutionOptions.Default,
        DefaultContentTypeBasedDeserialisersExclusions deserialisersExclusions = DefaultContentTypeBasedDeserialisersExclusions.None,
        TestHttpMethod requestMethod = TestHttpMethod.Post)
    {
        // Arrange
        using var tempFolder = new TemporaryFolderFactory().Create();
        using var currentFolder = new DisposableCurrentDirectory(tempFolder.FullPath);

        var capturedUrls = new List<string>();
        var provider = new ServiceCollection()
            .AddSingleton<IHomeFolderProvider>(sp => new TestHomeFolderProvider(Path.Combine(tempFolder.FullPath, "user-home")))       
            .AddMockHttpMessageHandler()
            .Configure<TestOptions>(c =>
            {
                c.Uri = "http://nowhere.com/?apikey=my-secret-thingy&api_key=other-one";
                c.Headers.AddRange([
                    new("X-Remove", "remove it"),
                    new("X-Other-Remove", "duh"),
                    new("Authorization", "Bearer my-fully-secret-thingy")              
                ]);

                if (requestContentType is not null) c.RequestContentType = requestContentType;
                if (requestContent is not null) c.RequestContent = requestContent;
                c.Method = requestMethod == TestHttpMethod.Get ? HttpMethod.Get : HttpMethod.Post;
            })
            .ConfigureDumpHandlerErrorLogging(o => o.ReThrowException = false)
            .AddHttpClient<MyTestClient>()
            .AddRequestAndResponseCapturing(c => c
                .AddDumpCapturingHandlerWithDefaults(configurator: c => c
                    .AddContentTypeBasedDeserialiser<ErrorThrowingContentTypeDeserialiser>()
                    .AddContentTypeBasedDeserialiser<HtmlDeserialiser>())
                .AddDumpCapturingHandler(c => c
                    .AddDefaultDumpHandler(c => c.UseOutputFolderResolverDelegate(c => "second-handler")))
                .AddDumpCapturingHandler(c => c
                    .AddDefaultContentTypeBasedDeserialisers()
                    .AddDefaultDumpHandler(c => c.UseOutputFolderResolverDelegate(c => "third-handler")))                    
            )
            .AddRequestAndResponseCapturing(c => c
                .AddDumpCapturingHandler(c => c
                    .AddContentTypeBasedDeserialiser<HtmlDeserialiser>()
                    .AddDumpHandler<ErrorThrowingDumpHandler>()
                    .AddDefaultContentTypeBasedDeserialisers(deserialisersExclusions)
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
                    .AddHeaderMapper<ErrorThrowingHeaderMapper>()                    
                    .AddQueryParameterObfuscatingUriTransformer(c => c.UseDefaultObfuscator(c => c.ObfuscatingSuffix = "**"))
                    .AddUriTransformer<TestUriTransformer>()
                    .AddUriTransformer<ErrorThrowingUriTransformer>()
                    .AddDefaultDumpHandler(c => c
                        .ExecuteIf(outputFolderResolution == ResolutionOptions.Default, c => c.UseHomeFolderForOutput(c => c.SubFolder = "test-output/{host}"))
                        .ExecuteIf(outputFolderResolution == ResolutionOptions.Delegate, c => c.UseOutputFolderResolverDelegate(_ => "test-output2"))
                        .ExecuteIf(outputFolderResolution == ResolutionOptions.Custom, c => c.UseOutputFolderResolver<TestOutputFolderResolver>())

                        .ExecuteIf(filenameResolution == ResolutionOptions.Default, c => c.UseDefaultFilenameResolver(c => c.TakePathSegments = 20))
                        .ExecuteIf(filenameResolution == ResolutionOptions.Delegate, c => c.UseFilenameResolverDelegate(_ => $"{Guid.NewGuid()}"))
                        .ExecuteIf(filenameResolution == ResolutionOptions.Custom, c => c.UseFilenameResolver<TestFilenameResolver>())
                    )
                    .AddDumpHandlerDelegate(dump => capturedUrls.Add(dump.Request.RequestUri.ToString()))
                )
            )
            .Services
            .BuildServiceProvider();
        
        var options = provider.GetRequiredService<IOptions<TestOptions>>();
        var client = provider.GetRequiredService<MyTestClient>();
        var mockHttpMessageHandler = provider.GetMockHttpHandler();
        mockHttpMessageHandler.When(options.Value.Uri)
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
                c.Uri = "http://nowhere.com/?apikey=my-secret-thingy";
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
        mockHttpMessageHandler.When(options.Value.Uri)
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
    public async Task GivenAnErroringClient_ItShouldProvideAnException()
    {
        ExceptionDump e = null;
        var provider = new ServiceCollection()
            .AddHttpClient<MyTestClient>()
            .AddRequestAndResponseCapturing(c => c
                .AddDumpCapturingHandler(c => c
                    .AddDumpHandlerDelegate(d => e = d.Exception))
            )
            .Services
            .AddMockHttpMessageHandler()
            .BuildServiceProvider();

        var client = provider.GetRequiredService<MyTestClient>();

        var toRun = client.SendRequest;
        
        await toRun.Should().ThrowExactlyAsync<ArgumentException>();

        e.Should().NotBeNull();
    }

    [Test]
    public void GivenANullUriTransformer_ItShouldThrowAnArgumentNullException()
    {
        
        // Arrange
        var providerAction =  new Action(() => new ServiceCollection()
            .Configure<TestOptions>(c =>
            {
                c.Uri = "http://nowhere.com/?apikey=my";
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
}