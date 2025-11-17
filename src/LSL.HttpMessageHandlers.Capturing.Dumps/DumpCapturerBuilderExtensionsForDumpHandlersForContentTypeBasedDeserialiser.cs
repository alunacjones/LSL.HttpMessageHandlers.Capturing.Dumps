using LSL.HttpMessageHandlers.Capturing.Core;
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