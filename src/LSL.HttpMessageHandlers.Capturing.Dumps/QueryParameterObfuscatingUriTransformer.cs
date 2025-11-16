using System;
using System.Linq;
using System.Web;
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
    public Uri Transform(Uri source)
    {
        var builder = new UriBuilder(source);
        var queryString = HttpUtility.ParseQueryString(builder.Query);

        foreach (var toObfuscate in _options.Value.ParametersToObfuscate)
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

        builder.Query = queryString.ToString();

        return builder.Uri;
    }
}