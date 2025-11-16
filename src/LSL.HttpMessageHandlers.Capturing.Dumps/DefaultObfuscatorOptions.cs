using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Options for the default obfuscator
/// </summary>
public class DefaultObfuscatorOptions
{   
    /// <summary>
    /// The number of characters from the start of a string to leave un-obfuscated
    /// </summary>
    /// <remarks>
    /// Defaults to 5
    /// </remarks>
    public int NumberOfCharactersToKeepClear { get; set; } = 5;

    /// <summary>
    /// The string to use as the obfuscated section.
    /// </summary>
    /// <remarks>
    /// Defaults to `*****`
    /// </remarks>
    public string ObfuscatingSuffix { get; set; } = "*****";

    /// <inheritdoc/>
    internal ServiceProviderBasedFactory<IObfuscator> ObfuscatorFactory { get; set; } = sp => sp.GetRequiredService<DefaultObfuscator>();
}