using System;
using System.Threading.Tasks;
using FluentAssertions;

namespace LSL.HttpMessageHandlers.Capturing.Dumps.Tests;

public class ErrorLoggingExecutorTests
{
    [Test]
    public void ExecuteWithErrorHandling_WhenAnExceptionIsNotThrown_ItShouldNotExecuteTheErrorHandler()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        new ErrorLoggingExecutor()
            .ExecuteWithErrorHandling(() => wasExecuted = true, e => capturedException = e);

        wasExecuted.Should().BeTrue();
        capturedException.Should().BeNull();
    }

    [Test]
    public void ExecuteWithErrorHandling_WhenAnExceptionIsNotThrown_ItShouldNotExecuteTheErrorHandlerAndReturnTheResult()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        var result = new ErrorLoggingExecutor()
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

        new ErrorLoggingExecutor()
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
    public void ExecuteWithErrorHandling_WhenAnExceptionIsThrown_ItShouldExecuteTheErrorHandlerAndReturnTheDefaultValue()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        var result = new ErrorLoggingExecutor()
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
    public async Task ExecuteAsyncWithErrorHandling_WhenAnExceptionIsNotThrown_ItShouldNotExecuteTheErrorHandler()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        await new ErrorLoggingExecutor()
            .ExecuteAsyncWithErrorHandling(async () => wasExecuted = true, e => capturedException = e);

        wasExecuted.Should().BeTrue();
        capturedException.Should().BeNull();
    }

    [Test]
    public async Task ExecuteAsyncWithErrorHandling_WhenAnExceptionIsThrown_ItShouldExecuteTheErrorHandler()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        await new ErrorLoggingExecutor()
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
    public async Task ExecuteAsyncWithErrorHandling_WhenAnExceptionIsThrown_ItShouldExecuteTheErrorHandlerAndReturnTheDefaultValue()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        var result = await new ErrorLoggingExecutor()
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
    public async Task ExecuteAsyncWithErrorHandling_WhenAnExceptionIsThrown_ItShouldNotExecuteTheErrorHandlerAndReturnTheDefaultValue()
    {
        var wasExecuted = false;
        Exception capturedException = null;

        var result = await new ErrorLoggingExecutor()
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
}