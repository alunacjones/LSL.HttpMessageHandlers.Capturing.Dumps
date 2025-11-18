using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace LSL.HttpMessageHandlers.Capturing.Dumps;

internal class DefaultDumpHandler : BaseDumpHandler
{
    private readonly Lazy<DefaultDumpHandlerOptions> _options;
    private readonly Lazy<IOutputFolderResolver> _outputFolderResolver;
    private readonly IDirectoryCreator _directoryCreator;
    private readonly Lazy<IFilenameResolver> _filenameResolver;

    public DefaultDumpHandler(
        IOptionsSnapshot<DefaultDumpHandlerOptions> optionsSnapshot,
        ICompoundFactory compoundFactory,
        IDirectoryCreator directoryCreator)
    {
        _options = new(() => optionsSnapshot.Get(Name));
        _outputFolderResolver = new(() => 
            compoundFactory.CreateBuilder([_options.Value.OutputFolderResolverFactory])
            .Services
            .Single());
        _directoryCreator = directoryCreator;
        _filenameResolver = new(() => compoundFactory.CreateBuilder([_options.Value.FilenameResolverFactory]).Services.Single());
    }

    public override Task Dump(RequestAndResponseDump requestAndResponseDump)
    {
        // TODO
        var resolvedFolder = Path.GetFullPath(_outputFolderResolver.Value.ResolveOutputFolder(requestAndResponseDump));
        var outputFolder = _directoryCreator.CreateDirectory(resolvedFolder);
        
        var options = _options.Value;

        var toOutput = JsonSerializer.Serialize(requestAndResponseDump, options.JsonSerializerOptions);
        File.WriteAllText(Path.Combine(
            outputFolder, 
            $"{_filenameResolver.Value.ResolveFilenameWithNoExtension(requestAndResponseDump)}.json"), 
            toOutput);
        
        return Task.CompletedTask;
    }
}

/// <summary>
/// Directory creation abstraction
/// </summary>
public interface IDirectoryCreator
{
    /// <summary>
    /// Creates the directory (and any subfolders) and returns the full path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public string CreateDirectory(string path);
}

internal class DirectoryCreator : IDirectoryCreator
{
    public string CreateDirectory(string path) => Directory.CreateDirectory(path).FullName;
}
