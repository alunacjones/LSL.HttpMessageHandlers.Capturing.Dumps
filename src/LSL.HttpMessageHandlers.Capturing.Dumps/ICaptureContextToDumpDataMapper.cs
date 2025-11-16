using System;
using LSL.HttpMessageHandlers.Capturing.Core;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Capture context to dump data mapper interface
/// </summary>
public interface ICaptureContextToDumpDataMapper
{
    /// <summary>
    /// Maps a <see cref="CaptureContext"/> to a <see cref="RequestAndResponseDump"/>
    /// using the given <see cref="DumpCapturingOptions"/>
    /// </summary>
    /// <param name="captureContext"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    RequestAndResponseDump Map(CaptureContext captureContext, IResolvedDumpCapturerOptions options);
}
