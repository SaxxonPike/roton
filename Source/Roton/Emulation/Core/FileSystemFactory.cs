using System.Collections.Generic;
using System.Reflection;
using Roton.Emulation.Core.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Core;

internal sealed class FileSystemFactory(IAssemblyResourceService assemblyResourceService)
    : IFileSystemFactory
{
    public IFileSystem CreateAggregate(IEnumerable<IFileSystem> fileSystems) =>
        new AggregateFileSystem(fileSystems);

    public IFileSystem CreateAssemblyResourceRoot(Assembly assembly) =>
        assemblyResourceService.GetFromAssembly(assembly).Root;

    public IFileSystem CreateAssemblyResourceSystem(Assembly assembly) =>
        assemblyResourceService.GetFromAssembly(assembly).Root;

    public IFileSystem CreateDisk(string basePath)
    {
        throw new System.NotImplementedException();
    }
}