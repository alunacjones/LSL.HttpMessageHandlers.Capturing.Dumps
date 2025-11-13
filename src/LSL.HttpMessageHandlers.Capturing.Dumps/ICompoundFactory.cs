using System.Collections.Generic;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal interface ICompoundFactory
{
    ICompoundFactoryBuilder<TService> CreateBuilder<TService>(IEnumerable<ServiceProviderBasedFactory<TService>> factories);
}
