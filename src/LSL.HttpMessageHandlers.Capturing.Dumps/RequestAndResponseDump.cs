namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Request and response dump
/// </summary>
public class RequestAndResponseDump
{
    /// <summary>
    /// The duration in seconds of the HTTP call
    /// </summary>
    public double DurationInSeconds { get; set; }

    /// <summary>
    /// The request dump of the HTTP call
    /// </summary>
    public RequestDump Request { get; set; } = default!;

    /// <summary>
    /// The response dup of the HTTP call. Is <see langword="null"/> if the send failed
    /// </summary>
    public ResponseDump? Response { get; set; }

    /// <summary>
    /// If an exception is thrown on send, then this contains the exception
    /// </summary>
    public ExceptionDump? Exception { get; set; }
}
