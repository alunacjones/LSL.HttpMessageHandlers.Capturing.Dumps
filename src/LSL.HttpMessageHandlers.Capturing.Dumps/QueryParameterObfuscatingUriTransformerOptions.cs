using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Query parameter obfuscating URI transformer options
/// </summary>
public class QueryParameterObfuscatingUriTransformerOptions : IHaveAnObfuscatorFactory<QueryParameterObfuscatingUriTransformerOptions>
{
    /// <summary>
    /// A list of query string parameter keys to obfuscate
    /// </summary>
    public List<string> ParametersToObfuscate { get; set; } = [
        "apiKey",
        "apikey",
        "api-key"
    ];

    /// <inheritdoc/>
    internal ServiceProviderBasedFactory<IObfuscator> ObfuscatorFactory { get; set; } = sp => sp.GetRequiredService<DefaultObfuscator>();

    ServiceProviderBasedFactory<IObfuscator> IHaveAnObfuscatorFactory<QueryParameterObfuscatingUriTransformerOptions>.ObfuscatorFactory { get => ObfuscatorFactory; set => ObfuscatorFactory = value; }
    QueryParameterObfuscatingUriTransformerOptions IHaveAnObfuscatorFactory<QueryParameterObfuscatingUriTransformerOptions>.Options => this;
}
