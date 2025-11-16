namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Exposes a name property that maps to an option name
/// </summary>
public interface IHaveAName
{
    /// <summary>
    /// The name of the option
    /// </summary>
    public string Name { get;}
}