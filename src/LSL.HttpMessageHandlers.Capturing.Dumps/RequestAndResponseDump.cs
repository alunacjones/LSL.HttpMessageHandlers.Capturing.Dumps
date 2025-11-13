using System;
using System.Net.Http;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

public class RequestAndResponseDump
{
    public decimal DurationInSeconds { get; set; }
    public RequestDump Request { get; set; }
    public ResponseDump? Response { get; set; }
    public Exception? Exception { get; set; }
}

public class RequestDump : ContentAndHeadersDump
{
    public Uri RequestUri { get; set; }
    public HttpMethod HttpMethod { get; set; }
}

public class ResponseDump : ContentAndHeadersDump
{    
}