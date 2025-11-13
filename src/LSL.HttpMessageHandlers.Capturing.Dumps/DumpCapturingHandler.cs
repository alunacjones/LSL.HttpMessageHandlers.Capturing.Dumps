using System;
using System.Threading.Tasks;
using LSL.HttpMessageHandlers.Capturing.Core;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class DumpCapturingHandler(
    string name,
    IOptionsSnapshot<DumpCapturingOptions> optionsSnapshot,
    ICaptureContextToDumpDataMapper captureContextToDumpDataMapper,
    DumpCapturerOptionsResolver dumpCapturerOptionsResolver) : IAsyncRequestAndResponseCapturer
{
    private readonly Lazy<IResolvedDumpCapturerOptions> _resolvedDumpCapturerOptions = new(
        () => dumpCapturerOptionsResolver.Resolve(optionsSnapshot.Get(name))
    );

    public Task CaptureAsync(CaptureContext context) =>
        _resolvedDumpCapturerOptions.Value.Handler(captureContextToDumpDataMapper.Map(context, _resolvedDumpCapturerOptions.Value));
}
