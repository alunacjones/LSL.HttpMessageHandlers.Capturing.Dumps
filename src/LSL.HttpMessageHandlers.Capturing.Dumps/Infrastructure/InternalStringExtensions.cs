namespace LSL.HttpMessageHandlers.Capturing.Dumps.Infrastructure;

internal static class InternalStringExtensions
{
    public static string ReplaceVariables(this string source, RequestAndResponseDump requestAndResponseDump)
    {
        return source.Replace("{host}", requestAndResponseDump.Request.RequestUri.Host.MakeFilenameSafe());
    }
}