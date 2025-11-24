using System;
using Microsoft.Extensions.Configuration;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Abstraction for configuration a builders options
/// </summary>
/// <typeparam name="TOptions"></typeparam>
public interface IBuilderConfiguration<TOptions>
    where TOptions : class
{
    /// <summary>
    /// Configure <typeparamref name="TOptions"/> for the builder from a configuration section
    /// </summary>
    /// <param name="configuration"></param>
    void Using(IConfiguration configuration);

    /// <summary>
    /// COnfigure <typeparamref name="TOptions"/> for the builder using the given delegate
    /// </summary>
    /// <param name="configurator"></param>
    void Using(Action<TOptions> configurator);
}
