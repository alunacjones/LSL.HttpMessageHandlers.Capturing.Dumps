[![Build status](https://img.shields.io/appveyor/ci/alunacjones/lsl-httpmessagehandlers-capturing-dumps.svg)](https://ci.appveyor.com/project/alunacjones/lsl-httpmessagehandlers-capturing-dumps)
[![Coveralls branch](https://img.shields.io/coverallsCoverage/github/alunacjones/LSL.HttpMessageHandlers.Capturing.Dumps)](https://coveralls.io/github/alunacjones/LSL.HttpMessageHandlers.Capturing.Dumps)
[![NuGet](https://img.shields.io/nuget/v/LSL.HttpMessageHandlers.Capturing.Dumps.svg)](https://www.nuget.org/packages/LSL.HttpMessageHandlers.Capturing.Dumps/)

# LSL.HttpMessageHandlers.Capturing.Dumps

Thi library provides quick and easy request and response dumping based on the [LSL.HttpMessageHandlers.Capturing.Core](https://www.nuget.org/packages/LSL.HttpMessageHandlers.Capturing.Core) library.

## Quick Start

The following code illustrates the addition of request and response dumping to a user profile folder based on the executing assembly:

> **NOTE**: `services` is an `IServiceCollection` instance

```csharp
services.AddHttpClient<MyTestClient>()            
    .AddRequestAndResponseCapturing(c => c
        .AddDumpCapturingHandlerWithDefaults()
    );

// On resolution of MyTestClient, all JSON request and responses 
// will be dumped into the current user's profile folder under the path
// .http-dumps/{executing-assembly-name}.
// {executing-assembly-name} is resolved using Assembly.GetExecutingAssembly()

```
<!-- HIDE -->
## Further Documentation

More in-depth documentation can be found [here](https://alunacjones.github.io/LSL.HttpMessageHandlers.Capturing.Dumps/)
<!-- END:HIDE -->