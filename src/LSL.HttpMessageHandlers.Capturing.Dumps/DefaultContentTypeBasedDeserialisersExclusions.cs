using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// An <see langword="enum"/> to exclude any of the default <see cref="IContentTypeBasedDeserialiser"/>s
/// </summary>
[Flags]
public enum DefaultContentTypeBasedDeserialisersExclusions
{
    /// <summary>
    /// Exclude none of the default content type based deserialisers
    /// </summary>
    None = 0,

    /// <summary>
    /// Exclude the <see cref="RedactingDeserialiser"/>
    /// </summary>
    RedactingDeserialiser = 1,

    /// <summary>
    /// Exclude the <see cref="JsonDeserialiser"/>
    /// </summary>
    JsonDeserialiser = 2,

    /// <summary>
    /// Exclude the <see cref="TextDeserialiser"/>
    /// </summary>
    TextDeserialiser = 4
}