using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// The exception dump
/// </summary>
public class ExceptionDump
{
    /// <summary>
    /// The exception message
    /// </summary>
    public string Message { get; set; } = default!;

    /// <summary>
    /// The inner exception dump
    /// </summary>
    public ExceptionDump InnerException { get; set; } = default!;

    /// <summary>
    /// The exception stack trace
    /// </summary>
    public string StackTrace { get; set; } = default!;

    /// <summary>
    /// The exception source
    /// </summary>
    public string Source { get; set; } = default!;

    /// <summary>
    /// Implicitly covert an exception to an <see cref="ExceptionDump"/>
    /// </summary>
    /// <param name="ex"></param>
    public static implicit operator ExceptionDump(Exception ex) => ex.ToExceptionDump();
}
