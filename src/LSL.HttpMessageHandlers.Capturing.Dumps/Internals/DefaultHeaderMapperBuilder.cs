using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Internals;

internal class DefaultHeaderMapperBuilder(string name, IServiceCollection services) : IDefaultHeaderMapperBuilder
{
    /// <inheritdoc/>
    public string Name => name;
    
    /// <inheritdoc/>
    public IServiceCollection Services => services;
}