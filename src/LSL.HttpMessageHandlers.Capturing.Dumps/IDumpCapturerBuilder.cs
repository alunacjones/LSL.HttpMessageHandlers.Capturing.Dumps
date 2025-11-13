using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

public interface IDumpCapturerBuilder
{
    /// <summary>
    /// The name of the dump capturer. Used for resolving options
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The service collection
    /// </summary>
    IServiceCollection Services { get; }    
}
