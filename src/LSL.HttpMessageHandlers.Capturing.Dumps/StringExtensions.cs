using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// StringExtensions
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Returns a substring even if the length is greater than the string size.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>Returns the full string is <paramref name="length"/> is greater or equal to the length of <paramref name="value"/></item>
    ///     <item>Returns <see langword="null"/> if <paramref name="value"/> is <see langword="null"/></item>
    /// </list>
    /// </remarks>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    [return: NotNullIfNotNull(nameof(value))]
    public static string? SafeSubstring(this string? value, int length) => value?.Substring(0,  Math.Min(value.Length, length));

    /// <summary>
    /// Makes a string safe for use as a filename.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string MakeFilenameSafe(this string source) => _safeFileNameRegex.Value.Replace(source, "_");

    private static readonly Lazy<Regex> _safeFileNameRegex = new(() =>
    {
        var regexString = string.Join("|", Path.GetInvalidFileNameChars()
            .Select(c => Regex.Escape(c.ToString())));

        return new Regex(regexString);
    });    
}