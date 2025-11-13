using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LSL.HttpMessageHandlers.Capturing.Core;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Dump capturing options
/// </summary>
public class DumpCapturingOptions
{
    internal List<ServiceProviderBasedFactory<BaseDumpHandler>> DumpHandlerFactories { get; } = [];
    internal List<ServiceProviderBasedFactory<IUriTransformer>> UrlTransformersFactories { get; } = [];
    internal ServiceProviderBasedFactory<IHeaderMapper> HeaderMapperFactory { get; set; } = sp => sp.GetRequiredService<IHeaderMapper>();
}

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
}
