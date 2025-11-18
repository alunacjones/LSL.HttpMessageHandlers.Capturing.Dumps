using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// DefaultDumpHandlerBuilderExtensionsForOutputFolderResolving
/// </summary>
public static class DefaultDumpHandlerBuilderExtensionsForOutputFolderResolving
{
    /// <summary>
    /// Uses the user's home folder as a dump location
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>The full folder is based on <see cref="HomeFolderOutputFolderResolverOptions.SubFolder"/></item>
    ///     <item>The default subfolder is the name of the executing assembly.</item>
    ///     <item>The subfolder can contain a path with multiple subfolders</item>
    /// </list>
    /// </remarks>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <returns></returns>
    public static IDefaultDumpHandlerBuilder UseHomeFolderForOutput(
        this IDefaultDumpHandlerBuilder source,
        Action<HomeFolderOutputFolderResolverOptions>? configurator = null)
    {
        source.Services
            .Configure(source.Name, configurator.MakeNullSafe());

        return source.UseOutputFolderResolver(sp => ActivatorUtilities.CreateInstance<HomeFolderOutputFolderResolver>(sp, source.Name));
    }

    /// <summary>
    /// Use the provided delegate to resolve an output path
    /// </summary>
    /// <param name="source"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static IDefaultDumpHandlerBuilder UseOutputFolderResolverDelegate(this IDefaultDumpHandlerBuilder source, Func<RequestAndResponseDump, string> provider) => 
        source.UseOutputFolderResolver(sp => ActivatorUtilities.CreateInstance<DelegatingOutputFolderResolver>(sp, provider));

    /// <summary>
    /// Use <typeparamref name="TProvider"/> as the output folder resolver
    /// </summary>
    /// <typeparam name="TProvider"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IDefaultDumpHandlerBuilder UseOutputFolderResolver<TProvider>(this IDefaultDumpHandlerBuilder source)
        where TProvider : class, IOutputFolderResolver
    {
        source.Services.TryAddTransient<TProvider>();
        return source.UseOutputFolderResolver(sp => sp.GetRequiredService<TProvider>());
    }

    /// <summary>
    /// Use the provided factory to build an <see cref="IOutputFolderResolver"/>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    public static IDefaultDumpHandlerBuilder UseOutputFolderResolver(this IDefaultDumpHandlerBuilder source, ServiceProviderBasedFactory<IOutputFolderResolver> factory)
    {
        source.Services.Configure<DefaultDumpHandlerOptions>(source.Name, c => c.OutputFolderResolverFactory = factory);
        return source;
    }
}
