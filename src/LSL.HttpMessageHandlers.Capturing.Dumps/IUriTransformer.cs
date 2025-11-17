using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// A <see cref="Uri"/> transformer
/// </summary>
public interface IUriTransformer
{
    /// <summary>
    /// Transforms a <see cref="UriBuilder"/>
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    UriBuilder Transform(UriBuilder source);
}