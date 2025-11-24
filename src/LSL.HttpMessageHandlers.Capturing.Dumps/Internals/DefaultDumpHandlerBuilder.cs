using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Internals;

internal class DefaultDumpHandlerBuilder(string name, IServiceCollection services) : IDefaultDumpHandlerBuilder
{
    public string Name => name;

    public IServiceCollection Services => services;
}