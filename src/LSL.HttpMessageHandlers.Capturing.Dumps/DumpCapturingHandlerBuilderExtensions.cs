using System;
using LSL.HttpMessageHandlers.Capturing.Core;
using LSL.HttpMessageHandlers.Capturing.Dumps.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Dump capturing handler builder extensions
/// </summary>
public static class DumpCapturingHandlerBuilderExtensions
{
    /// <summary>
    /// Adds the default dump capturer
    /// </summary>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <returns></returns>
    public static ICapturingHandlerBuilder AddDumpCapturingHandler(this ICapturingHandlerBuilder source, Action<IDumpCapturerBuilder>? configurator = null)
    {
        var name = source.AssertNotNull(nameof(source)).BuildUniqueName();
        var builder = new DefaultDumpCapturerBuilder(name, source.Services);

        configurator?.Invoke(builder);

        source
            .AddCapturingHandlerFactory(sp => new DumpCapturingHandler(
                name,
                sp.GetRequiredService<IOptionsSnapshot<DumpCapturingOptions>>(),
                sp.GetRequiredService<ICaptureContextToDumpDataMapper>(),
                sp.GetRequiredService<DumpCapturerOptionsResolver>())
            )
            .Services
            .FluentlyTryAddSingleton<IHomeFolderProvider, HomeFolderProvider>()
            .FluentlyTryAddSingleton<DumpCapturerOptionsResolver>()
            .FluentlyTryAddTransient<ICaptureContextToDumpDataMapper, CaptureContextToDumpDataMapper>()
            .FluentlyTryAddSingleton<ICompoundFactory, CompoundFactory>()
            .FluentlyTryAddTransient(sp => new DefaultObfuscator(Options.DefaultName, sp.GetRequiredService<IOptionsSnapshot<DefaultObfuscatorOptions>>()));

        return source;
    }

    /// <summary>
    /// Adds a dump capturing handler with default configuration values.
    /// </summary>
    /// <remarks>
    /// <para>The defaults are configured as follows (and can be augmented by any of the optional parameters):</para>
    /// <list type="bullet">
    ///     <item>
    ///         Adds the default dump capturer to output to the current user's profile folder under the `.http-output` 
    ///         folder and under a folder beneath that named after the executing assembly.
    ///     </item>
    ///     <item>Adds the default header capturer. It obfuscates the `Authorization` header on output</item>
    ///     <item>Adds the query parameter obfuscator so that it obfuscates these parameters: apikey, apiKey, api_key and api-key.</item>
    /// </list>
    /// </remarks>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <param name="defaultDumpHandlerConfigurator"></param>
    /// <param name="defaultHeaderMapperConfigurator"></param>
    /// <param name="queryParameterObfuscatingUriTransformerConfigurator"></param>
    /// <returns></returns>
    public static ICapturingHandlerBuilder AddDumpCapturingHandlerWithDefaults(
        this ICapturingHandlerBuilder source,
        Action<IDumpCapturerBuilder>? configurator = null,
        Action<DefaultDumpHandlerOptions>? defaultDumpHandlerConfigurator = null,
        Action<DefaultHeaderMapperOptions>? defaultHeaderMapperConfigurator = null,
        Action<QueryParameterObfuscatingUriTransformerOptions>? queryParameterObfuscatingUriTransformerConfigurator = null) => 
        source
            .AssertNotNull(nameof(source))
            .AddDumpCapturingHandler(c => c
                .AddDefaultDumpHandler(defaultDumpHandlerConfigurator.MakeNullSafe())
                .AddDefaultHeaderMapper(defaultHeaderMapperConfigurator.MakeNullSafe())
                .AddQueryParameterObfuscatingUriTransformer(queryParameterObfuscatingUriTransformerConfigurator.MakeNullSafe())
                .With(configurator.MakeNullSafe())
            );
}
