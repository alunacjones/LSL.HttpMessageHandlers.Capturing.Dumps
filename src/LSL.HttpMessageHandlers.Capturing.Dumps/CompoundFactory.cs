using System;
using System.Collections.Generic;
using System.Linq;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class CompoundFactory(IServiceProvider serviceProvider) : ICompoundFactory
{
    public ICompoundFactoryBuilder<TService> CreateBuilder<TService>(IEnumerable<ServiceProviderBasedFactory<TService>> factories) =>
        new CompoundFactoryBuilder<TService>([.. factories.Select(f => f(serviceProvider))]);
}
