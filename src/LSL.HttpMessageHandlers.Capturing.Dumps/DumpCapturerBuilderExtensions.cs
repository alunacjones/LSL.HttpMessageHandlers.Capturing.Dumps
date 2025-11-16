using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using LSL.HttpMessageHandlers.Capturing.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Dump capturer builder extensions
/// </summary>
public static class DumpCapturerBuilderExtensions 
{
    /// <summary>
    /// Adds the default dump handler
    /// </summary>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddDefaultDumpHandler(this IDumpCapturerBuilder source, Action<DefaultDumpHandlerOptions>? configurator = null)
    {
        configurator ??= o => {};
        var name = OptionsHelper.BuildUniqueName(source.Name);
        source.
            Services
            .Configure(name, configurator);

        return source.AddDumpHandler<DefaultDumpHandler>(source.Name);        
    }

    /// <summary>
    /// Adds the given dump handler implementation
    /// </summary>
    /// <typeparam name="TDumpHandler"></typeparam>
    /// <param name="source"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddDumpHandler<TDumpHandler>(this IDumpCapturerBuilder source, string? name = null)
        where TDumpHandler : BaseDumpHandler
    {
        name ??= OptionsHelper.BuildUniqueName(source.Name);
        source.Services
            .FluentlyTryAddTransient<TDumpHandler>()
            .Configure<DumpCapturingOptions>(source.Name, c => c.DumpHandlerFactories.Add(sp => sp.GetRequiredService<TDumpHandler>().With(d => d.Name = name)));
        return source;
    }

    /// <summary>
    /// Adds an async dump handling delegate
    /// </summary>
    /// <param name="source"></param>
    /// <param name="handlerDelegate"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddDumpHandlerDelegate(this IDumpCapturerBuilder source, Func<RequestAndResponseDump, Task> handlerDelegate)
    {
        var name = OptionsHelper.BuildUniqueName(source.Name);
        source.Services.Configure<DelegatingDumpHandlerOptions>(name, c => c.DumpDelegate = handlerDelegate);
        return source.AddDumpHandler<DelegatingDumpHandler>(name);
    }

    /// <summary>
    /// Adds a sync dump handling delegate
    /// </summary>
    /// <param name="source"></param>
    /// <param name="handlerDelegate"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddDumpHandlerDelegate(this IDumpCapturerBuilder source, Action<RequestAndResponseDump> handlerDelegate) =>
        source.AddDumpHandlerDelegate(dump =>
        {
            handlerDelegate(dump);
            return Task.CompletedTask; 
        });

    /// <summary>
    /// Uses the provided factory to resolve a <see cref="IHeaderMapper"/> to be used by the dump capturer
    /// </summary>
    /// <param name="source"></param>
    /// <param name="headerMapperFactory"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder UseHeaderMapper(this IDumpCapturerBuilder source, ServiceProviderBasedFactory<IHeaderMapper> headerMapperFactory)
    {
        source.Services.Configure<DumpCapturingOptions>(source.Name, c => c.HeaderMapperFactory = headerMapperFactory);
        return source;
    }

    /// <summary>
    /// Adds the default header mapper to the dump capturer
    /// </summary>
    /// <remarks>
    /// The default configuration will obfuscate the `Authorization` header.
    /// </remarks>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder UseDefaultHeaderMapper(this IDumpCapturerBuilder source, Action<DefaultHeaderMapperOptions>? configurator = null)
    {
        source.Services.Configure<DefaultHeaderMapperOptions>(c =>
        {
            c.HeadersToObfuscate = ["Authorization"];
            configurator?.Invoke(c);
        });

        return source;
    }

    /// <summary>
    /// Adds <typeparamref name="THeaderMapper"/> to the URI transformer list
    /// </summary>
    /// <typeparam name="THeaderMapper"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder UseHeaderMapper<THeaderMapper>(this IDumpCapturerBuilder source)
        where THeaderMapper : class, IHeaderMapper
    {
        source
            .UseHeaderMapper(sp => sp.GetRequiredService<THeaderMapper>())
            .Services
            .FluentlyTryAddTransient<THeaderMapper>();

        return source;        
    }

    /// <summary>
    /// Adds a factory that will build a <see cref="IUriTransformer"/> that will by used by the dump capturer.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddUriTransformer(this IDumpCapturerBuilder source, ServiceProviderBasedFactory<IUriTransformer> factory)
    {
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
    /// The default keys that are obfuscated are: apikey, apiKey and api-key
    /// </remarks>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddQueryParameterObfuscatingUriTransformer(
        this IDumpCapturerBuilder source,
        Action<QueryParameterObfuscatingUriTransformerOptions>? configurator = null)
    {
        configurator ??= o => { };
        var name = OptionsHelper.BuildUniqueName(source.Name);
        source.Services.Configure(name, configurator);

        return source.AddUriTransformer(sp => ActivatorUtilities.CreateInstance<QueryParameterObfuscatingUriTransformer>(
            sp, 
            name));
    }
}
