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
        configurator ??= o => { };
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
            .FluentlyTryAddSingleton<DumpCapturerOptionsResolver>()
            .FluentlyTryAddTransient<ICaptureContextToDumpDataMapper, CaptureContextToDumpDataMapper>()
            .FluentlyTryAddSingleton<ICompoundFactory, CompoundFactory>()
            .FluentlyTryAddTransient(sp => ActivatorUtilities.CreateInstance<DefaultHeaderMapper>(sp, Options.DefaultName))
            .FluentlyTryAddTransient(sp => new DefaultObfuscator(Options.DefaultName, sp.GetRequiredService<IOptionsSnapshot<DefaultObfuscatorOptions>>()))            
            .AddOptions<DefaultObfuscatorOptions>();

        return source;
    }
}


