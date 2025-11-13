using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// A <see cref="Uri"/> transformer
/// </summary>
public interface IUriTransformer
{
    /// <summary>
    /// Transforms a <see cref="Uri"/>
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    Uri Transform(Uri source);
}

