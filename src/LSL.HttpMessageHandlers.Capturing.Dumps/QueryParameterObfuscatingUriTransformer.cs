using System;
using System.Linq;
using System.Web;
using LSL.HttpMessageHandlers.Capturing.Dumps.Infrastructure;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class QueryParameterObfuscatingUriTransformer : IUriTransformer
{
    private readonly Lazy<IObfuscator> _obfuscator;
    private readonly Lazy<QueryParameterObfuscatingUriTransformerOptions> _options;

    public QueryParameterObfuscatingUriTransformer(
        string name,
        ICompoundFactory compoundFactory,
        IOptionsSnapshot<QueryParameterObfuscatingUriTransformerOptions> optionsSnapshot)
    {
        _options = new(() => optionsSnapshot.Get(name));
        _obfuscator = new Lazy<IObfuscator>(
            () => compoundFactory.CreateBuilder([_options.Value.ObfuscatorFactory]).Services.Single()
        );
    }

    /// <inheritdoc/>
    public UriBuilder Transform(UriBuilder source)
    {
        var queryString = HttpUtility.ParseQueryString(source.Query);

        foreach (var toObfuscate in _options.Value.ParametersToObfuscate.AssertNotNull(nameof(QueryParameterObfuscatingUriTransformerOptions.ParametersToObfuscate)))
        {
            var values = queryString.GetValues(toObfuscate);

            if (values is not null)
            {
                queryString.Remove(toObfuscate);
                queryString.AddMultiple(
                    toObfuscate, 
                    values.Select(v => _obfuscator.Value.Obfuscate(toObfuscate, v)));
            }
        }

        source.Query = queryString.ToString();

        return source;
    }
}