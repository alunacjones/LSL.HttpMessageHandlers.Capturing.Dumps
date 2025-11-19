namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// The response dump
/// </summary>
public class ResponseDump : ContentAndHeadersDump
{
    /// <summary>
    /// The HTTP status code returned by the response
    /// </summary>
    public int StatusCode { get; set; }
}