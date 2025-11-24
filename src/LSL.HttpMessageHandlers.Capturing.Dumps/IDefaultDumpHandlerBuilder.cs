using System;
using LSL.HttpMessageHandlers.Capturing.Dumps.Infrastructure;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Default dump handler builder
/// </summary>
public interface IDefaultDumpHandlerBuilder : IAmABuilderWithOptions<DefaultDumpHandlerOptions>
{
}

/// <summary>
/// DefaultDumpHandlerBuilderExtensions
/// </summary>
public static class DefaultDumpHandlerBuilderExtensions
{
    /// <summary>
    /// Configures the default header mapper options
    /// </summary>
    /// <param name="source"></param>
    /// <param name="builderConfigurator"></param>
    /// <returns></returns>    
    public static IDefaultDumpHandlerBuilder Configure(
        this IDefaultDumpHandlerBuilder source,
        Action<IBuilderConfiguration<DefaultDumpHandlerOptions>> builderConfigurator)
    {
        BuilderConfiguration.BuildAndConfigure(source, builderConfigurator.AssertNotNull(nameof(builderConfigurator)));
        return source;
    }
}