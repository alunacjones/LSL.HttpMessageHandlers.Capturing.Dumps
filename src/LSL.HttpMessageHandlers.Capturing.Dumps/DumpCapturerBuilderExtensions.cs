using System;
using System.Threading.Tasks;
using LSL.HttpMessageHandlers.Capturing.Core;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Dump capturer builder extensions
/// </summary>
public static class DumpCapturerBuilderExtensionsForDumpHandlers 
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
}