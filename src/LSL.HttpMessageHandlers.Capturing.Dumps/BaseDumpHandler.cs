using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Base dump handler is the base class for all dump handlers
/// </summary>
public abstract class BaseDumpHandler 
{
    /// <summary>
    /// The name of the handler. Used for resolving options if needed.
    /// </summary>
    public string Name { get; internal set; } = default!;

    /// <summary>
    /// The dump handler
    /// </summary>
    /// <param name="requestAndResponseDump"></param>
    /// <returns></returns>
    public abstract Task Dump(RequestAndResponseDump requestAndResponseDump);
}
