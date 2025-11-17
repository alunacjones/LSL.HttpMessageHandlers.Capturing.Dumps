using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class ImmediateOptionsSnapShot<T>(T options) : IOptionsSnapshot<T>
    where T : class
{
    [ExcludeFromCodeCoverage]
    public T Value => options;
    public T Get(string name) => options;
}