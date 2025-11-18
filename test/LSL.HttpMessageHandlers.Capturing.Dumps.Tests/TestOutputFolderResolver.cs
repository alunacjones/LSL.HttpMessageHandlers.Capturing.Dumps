namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class TestOutputFolderResolver : IOutputFolderResolver
{
    public string ResolveOutputFolder(RequestAndResponseDump requestAndResponseDump) => "custom";
}