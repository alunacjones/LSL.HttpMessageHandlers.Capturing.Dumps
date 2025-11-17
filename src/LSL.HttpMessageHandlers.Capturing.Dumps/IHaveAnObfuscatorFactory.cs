namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Interface that exposes an obfuscator factory
/// </summary>
public interface IHaveAnObfuscatorFactory<TOptions>
{
    /// <summary>
    /// A factory to create an <see cref="IObfuscator"/> instance
    /// </summary>
    ServiceProviderBasedFactory<IObfuscator> ObfuscatorFactory { get; set; }

    /// <summary>
    /// The underlying <typeparamref name="TOptions"/>
    /// </summary>
    TOptions Options { get; } 
}