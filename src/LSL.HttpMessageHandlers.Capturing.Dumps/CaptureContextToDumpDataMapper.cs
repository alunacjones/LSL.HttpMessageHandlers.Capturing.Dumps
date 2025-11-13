using System.Linq;
using LSL.HttpMessageHandlers.Capturing.Core;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class CaptureContextToDumpDataMapper : ICaptureContextToDumpDataMapper
{
    public RequestAndResponseDump Map(CaptureContext captureContext, IResolvedDumpCapturerOptions options)
    {
        var result = new RequestAndResponseDump()
        {
            Request = new()
            {
                RequestUri = options.UriTransformer(captureContext.Request.RequestUri),
                HttpMethod = captureContext.Request.Method,
                Content = new(),
                Headers = (captureContext.Request.Content is null 
                    ? captureContext.Request.Headers
                    : captureContext.Request.Content.Headers.Concat(captureContext.Request.Headers))
                    .OrderBy(h => h.Key)
            }
        };

        captureContext.WithRequestAndResponse((_, res) =>
            result.Response = new()
            {
                Content = new(),
                Headers = (res.Content.Headers is null
                    ? res.Headers
                    : res.Content.Headers.Concat(res.Headers))
                    .OrderBy(h => h.Key)
            }
        );

        captureContext.WithExceptionAndRequest((ex, _) => result.Exception = ex);

        return result;
    }
}