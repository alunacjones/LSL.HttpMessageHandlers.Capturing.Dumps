using System;
using System.Runtime.CompilerServices;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal static class ActionExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SafeInvoke<T>(this Action<T>? action, T parameter) => action?.Invoke(parameter);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Action<T> MakeNullSafe<T>(this Action<T>? configurator) => configuration => configurator.SafeInvoke(configuration);
}
