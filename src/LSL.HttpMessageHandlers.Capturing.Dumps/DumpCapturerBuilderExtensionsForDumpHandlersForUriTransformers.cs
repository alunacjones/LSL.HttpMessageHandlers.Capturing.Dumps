using System;
using LSL.HttpMessageHandlers.Capturing.Core;
using LSL.HttpMessageHandlers.Capturing.Dumps.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Dump capturer builder extensions for dump handlers for uri transformers
/// </summary>
public static class DumpCapturerBuilderExtensionsForDumpHandlersForUriTransformers
{
    /// <summary>
    /// Adds a factory that will build a <see cref="IUriTransformer"/> that will by used by the dump capturer.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddUriTransformer(this IDumpCapturerBuilder source, ServiceProviderBasedFactory<IUriTransformer> factory)
    {
        factory.AssertNotNull(nameof(factory)); 
        source.Services.Configure<DumpCapturingOptions>(source.Name, c => c.UrlTransformersFactories.Add(factory));
        return source;
    }

    /// <summary>
    /// Adds an <see cref="IUriTransformer" /> to the dump capturer
    /// </summary>
    /// <typeparam name="TTransformer"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddUriTransformer<TTransformer>(this IDumpCapturerBuilder source)
        where TTransformer : class, IUriTransformer
    {
        source.Services.TryAddTransient<TTransformer>();
        return source.AddUriTransformer(sp => sp.GetRequiredService<TTransformer>());
    }

    /// <summary>
    /// Adds a query parameter obfuscator.
    /// </summary>
    /// <remarks>
    /// The default keys that are obfuscated are: apikey, apiKey, api_key and api-key
    /// </remarks>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddQueryParameterObfuscatingUriTransformer(
        this IDumpCapturerBuilder source,
        Action<QueryParameterObfuscatingUriTransformerOptions>? configurator = null)
    {
        var name = OptionsHelper.BuildUniqueName(source.Name);
        source.Services.Configure(name, configurator.MakeNullSafe());

        return source.AddUriTransformer(sp => 
            ActivatorUtilities.CreateInstance<QueryParameterObfuscatingUriTransformer>(sp, name)
        );
    }    
}