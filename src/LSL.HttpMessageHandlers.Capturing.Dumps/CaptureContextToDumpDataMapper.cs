using System.Collections.Generic;
using System.Linq;
using LSL.HttpMessageHandlers.Capturing.Core;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class CaptureContextToDumpDataMapper : ICaptureContextToDumpDataMapper
{
    public RequestAndResponseDump Map(CaptureContext captureContext, IResolvedDumpCapturerOptions options)
    {
        var result = new RequestAndResponseDump()
        {
            DurationInSeconds = captureContext.TimeToRun.TotalSeconds,            
            Request = new()
            {
                RequestUri = options.UriTransformer(captureContext.Request.RequestUri),
                HttpMethod = captureContext.Request.Method.Method,
                Content = new(),
                Headers = options.HeaderMapper.MapHeaders((captureContext.Request.Content is null 
                    ? captureContext.Request.Headers
                    : captureContext.Request.Content.Headers.Concat(captureContext.Request.Headers))
                    .OrderBy(h => h.Key)
                    .ToHeaderDictionary())
            }
        };

        captureContext.WithRequestAndResponse((_, res) =>
            result.Response = new()
            {
                Content = new(),
                Headers = options.HeaderMapper.MapHeaders((res.Content.Headers is null
                    ? res.Headers
                    : res.Content.Headers.Concat(res.Headers))
                    .OrderBy(h => h.Key)
                    .ToHeaderDictionary())
            }
        );

        captureContext.WithExceptionAndRequest((ex, _) => result.Exception = ex);

        return result;
    }
}

internal static class HeadersExtensions
{
    public static IDictionary<string, IEnumerable<string>> ToHeaderDictionary(this IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers) => 
        headers.GroupBy(h => h.Key).ToDictionary(h => h.Key, h => h.SelectMany(v => v.Value));
}