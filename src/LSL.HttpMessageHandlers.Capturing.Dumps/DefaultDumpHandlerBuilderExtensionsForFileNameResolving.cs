using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// DefaultDumpHandlerBuilderExtensionsForFilenameResolving
/// </summary>
public static class DefaultDumpHandlerBuilderExtensionsForFilenameResolving
{
    /// <summary>
    /// Use the default filename resolver and optionally override the default options with <paramref name="configurator"/>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <returns></returns>
    public static IDefaultDumpHandlerBuilder UseDefaultFilenameResolver(
        this IDefaultDumpHandlerBuilder source,
        Action<DefaultFilenameResolverOptions>? configurator = null)
    {
         source.Services
            .Configure(source.Name, configurator.MakeNullSafe());

        return source.UseFilenameResolver(sp => ActivatorUtilities.CreateInstance<DefaultFilenameResolver>(sp, source.Name));       
    }

    /// <summary>
    /// Use the provided delegate to resolve an output path
    /// </summary>
    /// <param name="source"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static IDefaultDumpHandlerBuilder UseFilenameResolverDelegate(this IDefaultDumpHandlerBuilder source, Func<RequestAndResponseDump, string> provider) => 
        source.UseFilenameResolver(sp => ActivatorUtilities.CreateInstance<DelegatingFilenameResolver>(sp, provider));

    /// <summary>
    /// Use <typeparamref name="TProvider"/> as the output folder resolver
    /// </summary>
    /// <typeparam name="TProvider"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IDefaultDumpHandlerBuilder UseFilenameResolver<TProvider>(this IDefaultDumpHandlerBuilder source)
        where TProvider : class, IFilenameResolver
    {
        source.Services.TryAddTransient<TProvider>();
        return source.UseFilenameResolver(sp => sp.GetRequiredService<TProvider>());
    }

    /// <summary>
    /// Use the provided factory to build an <see cref="IFilenameResolver"/>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="factory"></param>
    /// <returns></returns>    
    public static IDefaultDumpHandlerBuilder UseFilenameResolver(this IDefaultDumpHandlerBuilder source, ServiceProviderBasedFactory<IFilenameResolver> factory)
    {
        source.Services.Configure<DefaultDumpHandlerOptions>(source.Name, c => c.FilenameResolverFactory = factory);
        return source;
    }
}
