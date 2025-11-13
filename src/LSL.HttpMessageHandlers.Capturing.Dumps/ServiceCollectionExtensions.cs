using System;
using Microsoft.Extensions.DependencyInjection.Extensions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection FluentlyTryAddTransient<TService>(this IServiceCollection source, Func<IServiceProvider, TService>? factory = null)
        where TService : class
    {
        if (factory is null)
        {
            source.TryAddTransient<TService>();
        }
        else
        {
            source.TryAddTransient(factory);            
        }
        
        return source;
    }

    public static IServiceCollection FluentlyTryAddTransient<TService, TImplementation>(this IServiceCollection source)
        where TService : class
        where TImplementation : class, TService
    {
        source.TryAddTransient<TService, TImplementation>();
        return source;
    }    

    public static IServiceCollection FluentlyTryAddSingleton<TService, TImplementation>(this IServiceCollection source)
        where TService : class
        where TImplementation : class, TService
    {
        source.TryAddSingleton<TService, TImplementation>();
        return source;
    }

    public static IServiceCollection FluentlyTryAddSingleton<TService>(this IServiceCollection source)
        where TService : class
    {
        source.TryAddSingleton<TService>();
        return source;
    }

}