using System.Collections.Generic;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Dump capturing options
/// </summary>
public class DumpCapturingOptions
{
    internal List<ServiceProviderBasedFactory<BaseDumpHandler>> DumpHandlerFactories { get; } = [];
    internal List<ServiceProviderBasedFactory<IUriTransformer>> UrlTransformersFactories { get; } = [];
    internal List<ServiceProviderBasedFactory<IHeaderMapper>> HeaderMapperFactories { get; set; } = [];
    internal List<ServiceProviderBasedFactory<IContentTypeBasedDeserialiser>> ContentTypeBasedDeserialiserFactories { get; set;} = [];
}