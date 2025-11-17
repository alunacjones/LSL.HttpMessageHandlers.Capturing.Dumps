using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Dump capturing options
/// </summary>
public class DumpCapturingOptions
{
    internal List<ServiceProviderBasedFactory<BaseDumpHandler>> DumpHandlerFactories { get; } = [];
    internal List<ServiceProviderBasedFactory<IUriTransformer>> UrlTransformersFactories { get; } = [];
    internal List<ServiceProviderBasedFactory<IHeaderMapper>> HeaderMapperFactories { get; set; } = [];
}