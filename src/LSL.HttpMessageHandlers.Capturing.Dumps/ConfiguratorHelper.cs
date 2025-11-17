using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal static class ConfiguratorHelper
{
    public static Action<T> MakeNullSafe<T>(this Action<T>? configurator) => configuration => configurator?.Invoke(configuration);
}
