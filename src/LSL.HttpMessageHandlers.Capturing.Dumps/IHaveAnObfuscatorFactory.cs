namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Interface that defines an obfuscator factory
/// </summary>
public interface IHaveAnObfuscatorFactory
{
    /// <summary>
    /// A factory to create an <see cref="IObfuscator"/> instance
    /// </summary>
    ServiceProviderBasedFactory<IObfuscator> ObfuscatorFactory { get; set; }
}