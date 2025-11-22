using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class ErrorLoggingExecutorTests
{
    [Test]
    public void ExecuteWithErrorHandling_WhenAnExceptionIsNotThrown_ItShouldNotExecuteTheErrorHandler()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        BuildSut()
            .ExecuteWithErrorHandling(() => wasExecuted = true, e => capturedException = e);

        wasExecuted.Should().BeTrue();
        capturedException.Should().BeNull();
    }

    [Test]
    public void ExecuteWithErrorHandling_WhenAnExceptionIsNotThrown_ItShouldNotExecuteTheErrorHandlerAndReturnTheResult()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        var result = BuildSut()
            .ExecuteWithErrorHandling(() => 
            { 
                wasExecuted = true;
                return 23;
            }, 
            e => capturedException = e,
            12);

        wasExecuted.Should().BeTrue();
        capturedException.Should().BeNull();
        result.Should().Be(23);
    }

    [Test]
    public void ExecuteWithErrorHandling_WhenAnExceptionIsThrown_ItShouldExecuteTheErrorHandler()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        BuildSut()
            .ExecuteWithErrorHandling(() => 
            { 
                wasExecuted = true;
                throw new InvalidOperationException();
            }, 
            e => capturedException = e);

        wasExecuted.Should().BeTrue();
        capturedException.Should().BeOfType<InvalidOperationException>();
    }   

    [Test]
    public void ExecuteWithErrorHandling_WhenAnExceptionIsThrown_ItShouldExecuteTheErrorHandlerAndReThrow()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        new Action(() => BuildSut(true)
            .ExecuteWithErrorHandling(() => 
            { 
                wasExecuted = true;
                throw new InvalidOperationException();
            }, 
            e => capturedException = e)
        ).Should().ThrowExactly<InvalidOperationException>();

        wasExecuted.Should().BeTrue();
        capturedException.Should().BeOfType<InvalidOperationException>();
    }       

    [Test]
    public void ExecuteWithErrorHandling_WhenAnExceptionIsThrown_ItShouldExecuteTheErrorHandlerAndReturnTheDefaultValue()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        var result = BuildSut()
            .ExecuteWithErrorHandling(() => 
            { 
                wasExecuted = true;
                throw new InvalidOperationException();
            }, 
            e => capturedException = e,
            12);

        wasExecuted.Should().BeTrue();
        capturedException.Should().BeOfType<InvalidOperationException>();
        result.Should().Be(12);
    }

    [Test]
    public void ExecuteWithErrorHandlingWithAResult_WhenAnExceptionIsThrown_ItShouldExecuteTheErrorHandlerAndReThrow()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        new Action(() => BuildSut(true)
            .ExecuteWithErrorHandling(() => 
            { 
                wasExecuted = true;
                throw new InvalidOperationException();
            }, 
            e => capturedException = e,
            12)
        ).Should().ThrowExactly<InvalidOperationException>();

        wasExecuted.Should().BeTrue();
        capturedException.Should().BeOfType<InvalidOperationException>();
    }    

    [Test]
    public async Task ExecuteAsyncWithErrorHandling_WhenAnExceptionIsNotThrown_ItShouldNotExecuteTheErrorHandler()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        await BuildSut()
            .ExecuteAsyncWithErrorHandling(async () => wasExecuted = true, e => capturedException = e);

        wasExecuted.Should().BeTrue();
        capturedException.Should().BeNull();
    }

    [Test]
    public async Task ExecuteAsyncWithErrorHandling_WhenAnExceptionIsThrown_ItShouldExecuteTheErrorHandler()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        await BuildSut()
            .ExecuteAsyncWithErrorHandling(async () => 
            { 
                wasExecuted = true;
                throw new InvalidOperationException();
            }, 
            e => capturedException = e);

        wasExecuted.Should().BeTrue();
        capturedException.Should().BeOfType<InvalidOperationException>();
    }

    [Test]
    public async Task ExecuteAsyncWithErrorHandling_WhenAnExceptionIsThrown_ItShouldExecuteTheErrorHandlerAndReThrow()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        var toRun = async() => await BuildSut(true)
            .ExecuteAsyncWithErrorHandling(async () => 
            { 
                wasExecuted = true;
                throw new InvalidOperationException();
            }, 
            e => capturedException = e);

        await toRun.Should().ThrowExactlyAsync<InvalidOperationException>();
        wasExecuted.Should().BeTrue();
        capturedException.Should().BeOfType<InvalidOperationException>();
    }       

    [Test]
    public async Task ExecuteAsyncWithErrorHandling_WhenAnExceptionIsThrown_ItShouldExecuteTheErrorHandlerAndReturnTheDefaultValue()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        var result = await BuildSut()
            .ExecuteAsyncWithErrorHandling(async () => 
            { 
                wasExecuted = true;
                throw new InvalidOperationException();
            }, 
            e => capturedException = e,
            12);

        wasExecuted.Should().BeTrue();
        capturedException.Should().BeOfType<InvalidOperationException>();
        result.Should().Be(12);
    }

    [Test]
    public async Task ExecuteAsyncWithErrorHandling_WhenAnExceptionIsThrown_ItShouldExecuteTheErrorHandlerAndReThrowTheException()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        var toRun = async () => await BuildSut(true)
            .ExecuteAsyncWithErrorHandling(async () => 
            { 
                wasExecuted = true;
                throw new InvalidOperationException();
            }, 
            e => capturedException = e,
            12);

        await toRun.Should().ThrowExactlyAsync<InvalidOperationException>();

        wasExecuted.Should().BeTrue();
        capturedException.Should().BeOfType<InvalidOperationException>();
    }    

    [Test]
    public async Task ExecuteAsyncWithErrorHandling_WhenAnExceptionIsThrown_ItShouldNotExecuteTheErrorHandlerAndReturnTheDefaultValue()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        var result = await BuildSut()
            .ExecuteAsyncWithErrorHandling(async () => 
            { 
                wasExecuted = true;
                return 23;
            }, 
            e => capturedException = e,
            12);

        wasExecuted.Should().BeTrue();
        capturedException.Should().BeNull();
        result.Should().Be(23);
    }

    private static ErrorLoggingExecutor BuildSut(bool reThrow = false) => new(new TestSnapshot(reThrow));

    public class TestSnapshot(bool reThrow) : IOptionsSnapshot<ErrorLoggingExecutorOptions>
    {
        public ErrorLoggingExecutorOptions Value => new() { ReThrowException = reThrow };

        public ErrorLoggingExecutorOptions Get(string name)
        {
            throw new NotImplementedException();
        }
    }
}