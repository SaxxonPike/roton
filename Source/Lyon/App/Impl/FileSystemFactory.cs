using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Lyon.App.Impl;

[Context(Context.Startup)]
public sealed class FileSystemFactory : IFileSystemFactory
{
    private readonly IAssemblyResourceService _assemblyResourceService;

    public FileSystemFactory(IAssemblyResourceService assemblyResourceService)
    {
        _assemblyResourceService = assemblyResourceService;
    }
        
    public IFileSystem Create(string path)
    {
        return new AggregateFileSystem(new[]
        {
            new DiskFileSystem(path),
            _assemblyResourceService.GetFromAssemblyOf<IEngine>().Root
        });
    }
}