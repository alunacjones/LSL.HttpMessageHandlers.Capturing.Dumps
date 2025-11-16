using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Default header mapper options
/// </summary>
public class DefaultHeaderMapperOptions
{
    /// <summary>
    /// A list of headers to obfuscate
    /// </summary>
    public ICollection<string> HeadersToObfuscate { get; set; } = [];

    /// <summary>
    /// A list of headers to remove
    /// </summary>
    public ICollection<string> HeadersToRemove { get; set; } = [];

    internal ServiceProviderBasedFactory<IObfuscator> ObfuscatorFactory { get; } = sp => sp.GetRequiredService<DefaultObfuscator>();
}
