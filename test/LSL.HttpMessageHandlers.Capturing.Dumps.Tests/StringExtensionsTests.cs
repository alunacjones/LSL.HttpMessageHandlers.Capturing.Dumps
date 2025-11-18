using FluentAssertions;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class StringExtensionsTests
{
    [TestCase(null, 100, null)]
    [TestCase("", 100, "")]
    [TestCase("a-string", 100, "a-string")]
    [TestCase("a-string", 4, "a-st")]
    public void SafeSubString_GivenAValue_ItShouldProduceTheExpectedResult(string input, int length, string expectedOutput)
    {
        input.SafeSubstring(length).Should().Be(expectedOutput);
    }
}