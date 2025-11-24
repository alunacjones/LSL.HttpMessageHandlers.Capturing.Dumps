using System;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using LSL.HttpMessageHandlers.Capturing.Core;
using LSL.HttpMessageHandlers.Capturing.Dumps.Internals;
using Microsoft.Extensions.DependencyInjection;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Dump capturer builder extensions for dump handlers for content type based deserialiser
/// </summary>
public static class DumpCapturerBuilderExtensionsForDumpHandlersForContentTypeBasedDeserialiser
{

    /// <summary>
    /// Adds a factory to build a content type based deserialiser
    /// </summary>
    /// <param name="source"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddContentTypeBasedDeserialiser(this IDumpCapturerBuilder source, ServiceProviderBasedFactory<IContentTypeBasedDeserialiser> factory)
    {
        source.Services.Configure<DumpCapturingOptions>(source.Name, c => c.ContentTypeBasedDeserialiserFactories.Add(factory));
        return source;
    }

    /// <summary>
    /// Adds an asynchronous content type deserialiser delegate
    /// </summary>
    /// <param name="source"></param>
    /// <param name="delegate"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddAsyncContentTypeBasedDeserialiserDelegate(this IDumpCapturerBuilder source, Func<HttpContent, Task<JsonNode?>> @delegate) => 
        source.AddContentTypeBasedDeserialiser(sp => ActivatorUtilities.CreateInstance<DelegatingContentTypeBasedDeserialiser>(sp, @delegate));

    /// <summary>
    /// Adds a synchronous content type deserialiser delegate
    /// </summary>
    /// <param name="source"></param>
    /// <param name="delegate"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddContentTypeBasedDeserialiserDelegate(this IDumpCapturerBuilder source, Func<HttpContent, JsonNode?> @delegate) => 
        source.AddAsyncContentTypeBasedDeserialiserDelegate(
            new Func<HttpContent, Task<JsonNode?>>(httpContent => Task.FromResult(@delegate(httpContent)))
        );

    /// <summary>
    /// Adds <typeparamref name="TDeserialiser"/> as a content type deserialiser
    /// </summary>
    /// <typeparam name="TDeserialiser"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddContentTypeBasedDeserialiser<TDeserialiser>(this IDumpCapturerBuilder source)
        where TDeserialiser : class, IContentTypeBasedDeserialiser
    {
        source.Services.FluentlyTryAddTransient<TDeserialiser>();
        return source.AddContentTypeBasedDeserialiser(sp => sp.GetRequiredService<TDeserialiser>());
    }

    /// <summary>
    /// Adds all the default content type deserialisers
    /// </summary>
    /// <remarks>
    ///     <para>The following list details each deserialiser that is added if no exclusions are provided</para>
    ///     <list type="bullet">
    ///         <item><see cref="JsonDeserialiser"/></item>
    ///         <item><see cref="TextDeserialiser"/></item>
    ///         <item><see cref="RedactingDeserialiser"/></item>
    ///     </list>
    /// </remarks>
    /// <param name="source"></param>
    /// <param name="defaultContentTypeBasedDeserialisersExclusions"></param>
    /// <returns></returns>
    public static IDumpCapturerBuilder AddDefaultContentTypeBasedDeserialisers(
        this IDumpCapturerBuilder source,
        DefaultContentTypeBasedDeserialisersExclusions defaultContentTypeBasedDeserialisersExclusions = DefaultContentTypeBasedDeserialisersExclusions.None) =>
        source.ConditionallyAddDefaultContentTypeBasedDeserialiser<JsonDeserialiser>(
                defaultContentTypeBasedDeserialisersExclusions, 
                DefaultContentTypeBasedDeserialisersExclusions.JsonDeserialiser)
            .ConditionallyAddDefaultContentTypeBasedDeserialiser<TextDeserialiser>(
                defaultContentTypeBasedDeserialisersExclusions, 
                DefaultContentTypeBasedDeserialisersExclusions.TextDeserialiser)
            .ConditionallyAddDefaultContentTypeBasedDeserialiser<RedactingDeserialiser>(
                defaultContentTypeBasedDeserialisersExclusions, 
                DefaultContentTypeBasedDeserialisersExclusions.RedactingDeserialiser);

    internal static IDumpCapturerBuilder ConditionallyAddDefaultContentTypeBasedDeserialiser<TDeserialiser>(
        this IDumpCapturerBuilder source,
        DefaultContentTypeBasedDeserialisersExclusions excludedValues,
        DefaultContentTypeBasedDeserialisersExclusions allowedValue)
        where TDeserialiser : class, IContentTypeBasedDeserialiser
    {
        if (excludedValues is DefaultContentTypeBasedDeserialisersExclusions.None || excludedValues.HasFlag(allowedValue) is false)
        {
            source.AddContentTypeBasedDeserialiser<TDeserialiser>();
        }

        return source;
    }
}