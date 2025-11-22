using System.IO;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class DirectoryCreator : IDirectoryCreator
{
    public string CreateDirectory(string path) => Directory.CreateDirectory(path).FullName;
}
