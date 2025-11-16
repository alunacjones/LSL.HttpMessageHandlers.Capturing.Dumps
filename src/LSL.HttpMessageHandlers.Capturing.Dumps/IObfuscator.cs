namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Obfuscator interface
/// </summary>
public interface IObfuscator
{
    /// <summary>
    /// Obfuscates the given string
    /// </summary>
    /// <param name="key"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public string Obfuscate(string key, string source);
}