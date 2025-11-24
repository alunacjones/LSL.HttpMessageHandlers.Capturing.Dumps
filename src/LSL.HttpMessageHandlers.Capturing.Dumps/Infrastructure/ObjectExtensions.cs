using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Infrastructure;

internal static class ObjectExtensions
{
    public static T With<T>(this T source, Action<T> configurator)
    {
        configurator(source);
        return source;
    }
}