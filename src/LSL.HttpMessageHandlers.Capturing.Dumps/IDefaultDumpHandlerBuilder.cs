using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Default dump handler builder
/// </summary>
public interface IDefaultDumpHandlerBuilder
{
    /// <summary>
    /// Name of the builder
    /// </summary>
    string Name { get;}

    /// <summary>
    /// The service collection
    /// </summary>
    IServiceCollection Services { get; }
}
