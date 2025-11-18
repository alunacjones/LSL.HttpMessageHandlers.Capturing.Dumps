using System;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

    public class TestFilenameResolver : IFilenameResolver
    {
        public string ResolveFilenameWithNoExtension(RequestAndResponseDump requestAndResponseDump) => $"{Guid.NewGuid()}-{Guid.NewGuid()}";
    }