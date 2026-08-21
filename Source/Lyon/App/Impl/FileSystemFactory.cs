using Roton;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Lyon.App.Impl;

[Context(Context.Startup)]
public sealed class FileSystemFactory(IAssemblyResourceService assemblyResourceService)
    : IFileSystemFactory
{
    public IFileSystem Create(string path)
    {
        return new AggregateFileSystem([
            new DiskFileSystem(path),
            assemblyResourceService.GetFromAssemblyOf<IEngine>().Root
        ]);
    }
}