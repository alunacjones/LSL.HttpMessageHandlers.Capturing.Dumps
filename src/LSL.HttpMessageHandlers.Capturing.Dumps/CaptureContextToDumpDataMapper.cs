using System.Linq;
using System.Threading.Tasks;
using LSL.HttpMessageHandlers.Capturing.Core;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class CaptureContextToDumpDataMapper : ICaptureContextToDumpDataMapper
{
    public async Task<RequestAndResponseDump> Map(CaptureContext captureContext, IResolvedDumpCapturerOptions options)
    {
        var result = new RequestAndResponseDump()
        {
            DurationInSeconds = captureContext.TimeToRun.TotalSeconds,            
            Request = new()
            {
                RequestUri = options.UriTransformer(captureContext.Request.RequestUri),
                HttpMethod = captureContext.Request.Method.Method,
                Content = await options.ContentTypeDeserialiser.Deserialise(captureContext.Request.Content).ConfigureAwait(false),
                Headers = options.HeaderMapper((captureContext.Request.Content is null 
                    ? captureContext.Request.Headers
                    : captureContext.Request.Content.Headers.Concat(captureContext.Request.Headers))
                    .OrderBy(h => h.Key)
                    .ToHeaderDictionary())
            }
        };

        await captureContext.WithRequestAndResponseAsync(async (_, res) =>
            result.Response = new()
            {
                StatusCode = (int)res.StatusCode,
                Content = await options.ContentTypeDeserialiser.Deserialise(res.Content).ConfigureAwait(false),
                Headers = options.HeaderMapper((res.Content.Headers is null
                    ? res.Headers
                    : res.Content.Headers.Concat(res.Headers))
                    .OrderBy(h => h.Key)
                    .ToHeaderDictionary())
            }
        ).ConfigureAwait(false);

        captureContext.WithExceptionAndRequest((ex, _) => result.Exception = ex);

        return result;
    }
}