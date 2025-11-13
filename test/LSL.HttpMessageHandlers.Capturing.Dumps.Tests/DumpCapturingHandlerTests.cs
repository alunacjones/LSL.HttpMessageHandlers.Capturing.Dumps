using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using LSL.HttpMessageHandlers.Capturing.Dumps.Tests.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class DumpCapturingHandlerTests
{
    [Test]
    public async Task WhenCreated_ItShouldDumpTheExpectedData()
    {
        // Arrange
        var capturedUrls = new List<string>();
        var provider = new ServiceCollection()
            .AddMockHttpMessageHandler()
            .AddHttpClient<MyTestClient>()            
            .AddRequestAndResponseCapturing(c => c
                .AddDumpCapturingHandler(c => c
                    .AddDefaultDumpHandler()
                    .AddDumpHandlerDelegate(dump => capturedUrls.Add(dump.Request.RequestUri.ToString()))
                )
            )
            .Services
            .BuildServiceProvider();
        
        var client = provider.GetRequiredService<MyTestClient>();
        var mockHttpMessageHandler = provider.GetMockHttpHandler();
        mockHttpMessageHandler.When("http://nowhere.com")
            .Respond(HttpStatusCode.OK);

        // Act
        await client.SendRequest();

        // Assert
        using var assertionScope = new AssertionScope();
        
        capturedUrls.Should().HaveCount(1);
    }

    private class MyTestClient(HttpClient httpClient)
    {
        public async Task SendRequest() => await httpClient.GetAsync("http://nowhere.com");
    }
}