namespace LSL.HttpMessageHandlers.Capturing.Dumps;

/// <summary>
/// Default filename resolver options
/// </summary>
public class DefaultFilenameResolverOptions
{
    /// <summary>
    /// The number of path segments to use from a request to build a filename
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>Defaults to -5</item>
    ///     <item>Negative values denote taking from the end of the URL</item>
    /// </list>
    /// </remarks>
    public int TakePathSegments { get; set; } = -5;

    /// <summary>
    /// The number of path segments to skip prior to taking from a request to build a filename
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>Defaults to 0</item>
    ///     <item>Negative values are made absolute before use</item>
    /// </list>
    /// </remarks>    
    public int SkipPathSegments { get; set; } = 0;
}