using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class DefaultDumpCapturerBuilder(string name, IServiceCollection services) : IDumpCapturerBuilder
{
    public string Name => name;

    public IServiceCollection Services => services;
}
