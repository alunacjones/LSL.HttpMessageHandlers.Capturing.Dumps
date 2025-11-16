using System;
using System.Diagnostics.CodeAnalysis;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal static class StringExtensions
{
    [return: NotNullIfNotNull(nameof(value))]
    public static string? SafeSubstring(this string? value, int length) => value?.Substring(0,  Math.Min(value.Length, length));
}