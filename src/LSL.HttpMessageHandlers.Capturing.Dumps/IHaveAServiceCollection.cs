using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Service collection container
/// </summary>
public interface IHaveAServiceCollection
{
    /// <summary>
    /// The service collection
    /// </summary>
    IServiceCollection Services { get; }            
}
