using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// An abstract builder to derive builders from
/// </summary>
public abstract class AbstractBuilder : IAmABuilder
{
    /// <inheritdoc/>
    public abstract string Name { get; }

    /// <inheritdoc/>
    public abstract IServiceCollection Services { get;}
}