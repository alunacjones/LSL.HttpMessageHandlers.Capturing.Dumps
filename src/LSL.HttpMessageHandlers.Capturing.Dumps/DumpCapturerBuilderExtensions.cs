using System;
using System.Threading.Tasks;
using LSL.HttpMessageHandlers.Capturing.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
    public static IDumpCapturerBuilder AddDefaultDumpHandler(this IDumpCapturerBuilder source, Action<DefaultDumpHandlerOptions>? configurator = null) => 
        source.AddDumpHandler<DefaultDumpHandler>(name => source.Services.Configure(name, configurator.MakeNullSafe()));

    /// <summary>
    /// Adds a dump handler factory to the dump capturer
    /// </summary>
    /// <param name="source"></param>
    /// <param name="factory"></param>
    /// <param name="withNameAction"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddDumpHandler(
        this IDumpCapturerBuilder source,
        ServiceProviderBasedFactory<BaseDumpHandler> factory,
        Action<string>? withNameAction = null)
    {
        var name = OptionsHelper.BuildUniqueName(source.Name);
        source.Services.Configure<DumpCapturingOptions>(source.Name, c => c.DumpHandlerFactories.Add(sp => factory(sp).With(d => d.Name = name)));
        withNameAction?.Invoke(name);
        return source;
    }

    /// <summary>
    /// Adds the given dump handler implementation
    /// </summary>
    /// <typeparam name="TDumpHandler"></typeparam>
    /// <param name="source"></param>
    /// <param name="withNameAction"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddDumpHandler<TDumpHandler>(this IDumpCapturerBuilder source, Action<string>? withNameAction = null)
        where TDumpHandler : BaseDumpHandler
    {
        source.Services.FluentlyTryAddTransient<TDumpHandler>();
        return source.AddDumpHandler(sp => sp.GetRequiredService<TDumpHandler>(), withNameAction);
    }

    /// <summary>
    /// Adds an async dump handling delegate
    /// </summary>
    /// <param name="source"></param>
    /// <param name="handlerDelegate"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddAsyncDumpHandlerDelegate(this IDumpCapturerBuilder source, Func<RequestAndResponseDump, Task> handlerDelegate) => 
        source.AddDumpHandler<DelegatingDumpHandler>(
            name => source.Services.Configure<DelegatingDumpHandlerOptions>(name, c => c.DumpDelegate = handlerDelegate)
        );

    /// <summary>
    /// Adds a synchronous dump handling delegate
    /// </summary>
    /// <param name="source"></param>
    /// <param name="handlerDelegate"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddDumpHandlerDelegate(this IDumpCapturerBuilder source, Action<RequestAndResponseDump> handlerDelegate) =>
        source.AddAsyncDumpHandlerDelegate(dump =>
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
    public static IDumpCapturerBuilder AddHeaderMapper(this IDumpCapturerBuilder source, ServiceProviderBasedFactory<IHeaderMapper> headerMapperFactory)
    {
        source.Services.Configure<DumpCapturingOptions>(source.Name, c => c.HeaderMapperFactories.Add(headerMapperFactory));
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
    public static IDumpCapturerBuilder AddDefaultHeaderMapper(this IDumpCapturerBuilder source, Action<DefaultHeaderMapperOptions>? configurator = null)
    {
        var name = OptionsHelper.BuildUniqueName(source.Name);
        source.Services.Configure<DefaultHeaderMapperOptions>(name, c => configurator?.Invoke(c));
        return source.AddHeaderMapper(sp => ActivatorUtilities.CreateInstance<DefaultHeaderMapper>(sp, name));
    }

    /// <summary>
    /// Adds <typeparamref name="THeaderMapper"/> to the dump capturer
    /// </summary>
    /// <typeparam name="THeaderMapper"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddHeaderMapper<THeaderMapper>(this IDumpCapturerBuilder source)
        where THeaderMapper : class, IHeaderMapper
    {
        source.Services.FluentlyTryAddTransient<THeaderMapper>();
        return source.AddHeaderMapper(sp => sp.GetRequiredService<THeaderMapper>());        
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
        var name = OptionsHelper.BuildUniqueName(source.Name);
        source.Services.Configure(name, configurator.MakeNullSafe());

        return source.AddUriTransformer(sp => 
            ActivatorUtilities.CreateInstance<QueryParameterObfuscatingUriTransformer>(sp, name)
        );
    }
}