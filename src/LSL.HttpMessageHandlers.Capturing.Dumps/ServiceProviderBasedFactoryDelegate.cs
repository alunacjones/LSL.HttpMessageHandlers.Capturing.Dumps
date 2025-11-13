using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// An <see cref="IServiceProvider"/> based factory to create instances of <typeparamref name="TResult"/>
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <param name="serviceProvider"></param>
/// <returns></returns>
public delegate TResult ServiceProviderBasedFactory<TResult>(IServiceProvider serviceProvider);