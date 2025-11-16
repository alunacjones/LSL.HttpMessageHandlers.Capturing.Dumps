using System;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class DefaultObfuscator(string name, IOptionsSnapshot<DefaultObfuscatorOptions> optionsSnapshot) : IObfuscator
{
    private readonly Lazy<DefaultObfuscatorOptions> _options = new(() => optionsSnapshot.Get(name));

    /// <inheritdoc/>
    public string Obfuscate(string key, string source) => 
        $"{source.SafeSubstring(_options.Value.NumberOfCharactersToKeepClear)}{_options.Value.ObfuscatingSuffix}";
}