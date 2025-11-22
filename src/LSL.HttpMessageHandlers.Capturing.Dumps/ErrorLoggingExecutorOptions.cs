namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Error logging executor options
/// </summary>
public class ErrorLoggingExecutorOptions
{
    /// <summary>
    /// If <see langword="true"/> then any caught exception will be re-thrown after logging the exception
    /// </summary>
    /// <remarks>
    /// Defaults to <see langword="false"/>
    /// </remarks>
    public bool ReThrowException { get; set; }
}