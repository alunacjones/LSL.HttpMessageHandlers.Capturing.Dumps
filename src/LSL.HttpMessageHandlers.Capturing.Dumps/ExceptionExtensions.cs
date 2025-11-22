using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal static class ExceptionExtensions
{
    public static ExceptionDump ToExceptionDump(this Exception exception) => 
        new()
        {
            Message = exception.Message,
            Source = exception.Source,
            StackTrace = exception.StackTrace,
            InnerException = exception.InnerException?.ToExceptionDump()!
        };
}