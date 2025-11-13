using System;
using System.Collections.Generic;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class CompoundFactoryBuilder<TService>(IEnumerable<TService> items) : ICompoundFactoryBuilder<TService>
{
    public IEnumerable<TService> Services => items;

    public ICompoundFactoryBuilder<TService, TContext> ForContext<TContext>() => new CompoundFactoryBuilder<TService, TContext>(Services);
}

internal class CompoundFactoryBuilder<TService, TContext>(IEnumerable<TService> services) : ICompoundFactoryBuilder<TService, TContext>
{
    public Func<TContext, TResult> Build<TResult>(Func<IEnumerable<TService>, Func<TContext, TResult>> factoryBuilder) => factoryBuilder(services);
}
