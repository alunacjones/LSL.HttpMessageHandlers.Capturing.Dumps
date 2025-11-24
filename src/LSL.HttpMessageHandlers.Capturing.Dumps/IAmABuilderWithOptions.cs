namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Abstractions for a builder that has options
/// </summary>
public interface IAmABuilderWithOptions<TOptions> : IAmABuilder
    where TOptions : class
{
}