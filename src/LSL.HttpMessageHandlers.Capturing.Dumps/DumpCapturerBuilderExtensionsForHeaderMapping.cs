using System;
using LSL.HttpMessageHandlers.Capturing.Core;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Dump capturer builder extensions for header mapping
/// </summary>
public static class DumpCapturerBuilderExtensionsForHeaderMapping
{
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
        source.Services.Configure(name, configurator.MakeNullSafe());
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
}