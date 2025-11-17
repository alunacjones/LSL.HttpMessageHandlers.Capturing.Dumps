using System;
using System.Collections.Generic;
using System.Linq;
using LSL.HttpMessageHandlers.Capturing.Dumps.Infrastructure;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// The default header mapper
/// </summary>
internal class DefaultHeaderMapper : IHeaderMapper
{
    private readonly IOptionsSnapshot<DefaultHeaderMapperOptions> _optionsSnapshot;
    private readonly DefaultHeaderMapperOptions _options;
    private readonly Lazy<IObfuscator> _obfuscator;

    public DefaultHeaderMapper(
        string name,
        IOptionsSnapshot<DefaultHeaderMapperOptions> optionsSnapshot,
        ICompoundFactory compoundFactory)
    {
        _optionsSnapshot = optionsSnapshot;
        _options = _optionsSnapshot.Get(name);
        _obfuscator = new Lazy<IObfuscator>(
            () => compoundFactory.CreateBuilder([_options.ObfuscatorFactory]).Services.Single()
        );
    }

    public IDictionary<string, IEnumerable<string>> MapHeaders(IDictionary<string, IEnumerable<string>> originalHeaders)
    {
        foreach (var toRemove in _options.HeadersToRemove.AssertNotNull(nameof(_options.HeadersToRemove)))
        {
            originalHeaders.Remove(toRemove);
        }

        foreach (var toObfuscate in _options.HeadersToObfuscate.AssertNotNull(nameof(_options.HeadersToObfuscate)))
        {
            if (originalHeaders.TryGetValue(toObfuscate, out var value))
            {
                originalHeaders[toObfuscate] = value.Select(v => _obfuscator.Value.Obfuscate(toObfuscate, v));
            }
        }

        return originalHeaders;
    }
}