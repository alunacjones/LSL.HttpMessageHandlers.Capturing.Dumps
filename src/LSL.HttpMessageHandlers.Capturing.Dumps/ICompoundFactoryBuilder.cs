using System;
using System.Collections.Generic;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal interface ICompoundFactoryBuilder<TService>
{
    ICompoundFactoryBuilder<TService, TContext> ForContext<TContext>();
    IEnumerable<TService> Services { get;}
}

internal interface ICompoundFactoryBuilder<TService, TContext>
{
    Func<TContext, TResult> Build<TResult>(Func<IEnumerable<TService>, Func<TContext, TResult>> factoryBuilder);
}
