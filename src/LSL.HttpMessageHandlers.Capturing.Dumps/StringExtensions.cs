using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal static class StringExtensions
{
    [return: NotNullIfNotNull(nameof(value))]
    public static string? SafeSubstring(this string? value, int length) => value?.Substring(0,  Math.Min(value.Length, length));

    public static string MakeFilenameSafe(this string source) => _safeFileNameRegex.Value.Replace(source, "_");

    public static string ReplaceVariables(this string source, RequestAndResponseDump requestAndResponseDump)
    {
        return source.Replace("{host}", requestAndResponseDump.Request.RequestUri.Host.MakeFilenameSafe());
    }

    private static readonly Lazy<Regex> _safeFileNameRegex = new(() =>
    {
        var regexString = string.Join("|", Path.GetInvalidFileNameChars()
            .Select(c => Regex.Escape(c.ToString())));

        return new Regex(regexString);
    });    
}