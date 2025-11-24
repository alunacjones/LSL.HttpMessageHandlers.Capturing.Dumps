using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Internals;

internal class BuilderConfiguration<TOptions>(IAmABuilderWithOptions<TOptions> self) : IBuilderConfiguration<TOptions>
    where TOptions : class
{
    public void Using(IConfiguration configuration) => self.Services.Configure<TOptions>(self.Name, configuration);
    public void Using(Action<TOptions> configurator) => self.Services.Configure(self.Name, configurator);    
}