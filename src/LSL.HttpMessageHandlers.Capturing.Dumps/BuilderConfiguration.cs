using System;
using LSL.HttpMessageHandlers.Capturing.Dumps.Infrastructure;
using LSL.HttpMessageHandlers.Capturing.Dumps.Internals;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Helpers to setup a builder configuration
/// </summary>
public static class BuilderConfiguration
{
    /// <summary>
    /// Builds and configures a build configuration instance
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="source"></param>
    /// <param name="builderConfigurator"></param>
    /// <returns></returns>
    public static IBuilderConfiguration<TOptions> BuildAndConfigure<TOptions>(IAmABuilderWithOptions<TOptions> source, Action<IBuilderConfiguration<TOptions>> builderConfigurator)
        where TOptions : class
    {
        var result = new BuilderConfiguration<TOptions>(source);
        builderConfigurator.AssertNotNull(nameof(builderConfigurator)).Invoke(result);
        return result;
    }
}

