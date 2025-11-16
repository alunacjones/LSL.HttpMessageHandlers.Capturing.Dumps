using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// The request dump
/// </summary>
public class RequestDump : ContentAndHeadersDump
{
    /// <summary>
    /// The request URI
    /// </summary>
    public Uri RequestUri { get; set; }= default!;

    /// <summary>
    /// The HTTP Method of the request
    /// </summary>
    public string HttpMethod { get; set; } = default!;
}
