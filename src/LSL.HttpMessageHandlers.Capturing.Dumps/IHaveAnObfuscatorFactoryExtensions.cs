using System;
using LSL.HttpMessageHandlers.Capturing.Core;
using LSL.HttpMessageHandlers.Capturing.Dumps.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// IHaveAnObfuscatorFactoryExtensions
/// </summary>
public static class IHaveAnObfuscatorFactoryExtensions
{
    /// <summary>
    /// Use the default obfuscator with an options configurator
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <returns></returns>
    public static TOptions UseDefaultObfuscator<TOptions>(this IHaveAnObfuscatorFactory<TOptions> source, Action<DefaultObfuscatorOptions> configurator)
    {
        var name = OptionsHelper.BuildUniqueName(Guid.NewGuid().ToString());
        var options = new DefaultObfuscatorOptions();
        configurator.AssertNotNull(nameof(configurator)).Invoke(options);
        source.ObfuscatorFactory = sp => ActivatorUtilities.CreateInstance<DefaultObfuscator>(sp, name, new ImmediateOptionsSnapShot<DefaultObfuscatorOptions>(options));

        return source.Options;
    }
}
