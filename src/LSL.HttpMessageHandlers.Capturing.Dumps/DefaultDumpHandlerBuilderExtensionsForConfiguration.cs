using System;
using LSL.HttpMessageHandlers.Capturing.Dumps.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// DefaultDumpHandlerBuilderExtensionsForConfiguration
/// </summary>
public static class DefaultDumpHandlerBuilderExtensionsForConfiguration
{
    /// <summary>
    /// Configures the default dump handler options with <paramref name="configurator"/>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <returns></returns>
    // public static IDefaultDumpHandlerBuilder ConfigureOptions(this IDefaultDumpHandlerBuilder source, Action<DefaultDumpHandlerOptions> configurator)
    // {
    //     configurator.AssertNotNull(nameof(configurator));
    //     source.Services.Configure(source.Name, configurator);
    //     return source;
    // }
}