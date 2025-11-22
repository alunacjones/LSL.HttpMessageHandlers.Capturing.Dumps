using System;
using LSL.HttpMessageHandlers.Capturing.Core;
using LSL.HttpMessageHandlers.Capturing.Dumps.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// ConfiguringServiceCollectionExtensions
/// </summary>
public static class ConfiguringServiceCollectionExtensions
{
    /// <summary>
    /// Configure the exception handling for all dump handlers
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configurator"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureDumpHandlerErrorLogging(this IServiceCollection services, Action<ErrorLoggingExecutorOptions> configurator) => 
        services.Configure(configurator.AssertNotNull(nameof(configurator)));
}