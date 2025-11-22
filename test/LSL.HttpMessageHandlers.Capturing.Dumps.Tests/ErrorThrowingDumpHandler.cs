using System;
using System.Threading.Tasks;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class ErrorThrowingDumpHandler : BaseDumpHandler
{
    public override Task Dump(RequestAndResponseDump requestAndResponseDump) => throw new NotImplementedException();
}