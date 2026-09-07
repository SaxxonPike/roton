using Roton;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Lyon.App.Impl;

/// <inheritdoc />
[Context(Context.Original)]
[Context(Context.Super)]
public sealed class FileSystemFactory(IAssemblyResourceService assemblyResourceService)
    : IFileSystemFactory
{
    /// <inheritdoc />
    public IFileSystem Create(string path)
    {
        // Prefer on-disk files before built-in resources.
        return new AggregateFileSystem([
            new DiskFileSystem(path),
            assemblyResourceService.GetFromAssemblyOf<IGame>().Root
        ]);
    }
}