using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// DefaultHeaderMapperBuilderExtensions
/// </summary>
public static class DefaultHeaderMapperBuilderExtensions
{
    /// <summary>
    /// Configures the default header mapper options
    /// </summary>
    /// <param name="source"></param>
    /// <param name="builderConfigurator"></param>
    /// <returns></returns>
    public static IDefaultHeaderMapperBuilder Configure(
        this IDefaultHeaderMapperBuilder source,
        Action<IBuilderConfiguration<DefaultHeaderMapperOptions>> builderConfigurator)
    {
        BuilderConfiguration.BuildAndConfigure(source, builderConfigurator);
        return source;
    }

    public static 
}