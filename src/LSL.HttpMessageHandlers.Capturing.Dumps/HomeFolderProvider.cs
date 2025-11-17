using System;
using System.Diagnostics.CodeAnalysis;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

[ExcludeFromCodeCoverage]
internal class HomeFolderProvider : IHomeFolderProvider
{
    public string GetHomeFolder()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    }
}