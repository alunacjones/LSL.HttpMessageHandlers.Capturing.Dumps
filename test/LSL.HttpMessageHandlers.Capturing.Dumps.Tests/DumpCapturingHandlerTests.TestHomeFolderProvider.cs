namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class TestHomeFolderProvider(string folderPath) : IHomeFolderProvider
{
    public string GetHomeFolder() => folderPath;
}