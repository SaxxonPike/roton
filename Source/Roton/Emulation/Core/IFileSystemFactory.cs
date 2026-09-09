using System.Collections.Generic;
using System.Reflection;

namespace Roton.Emulation.Core;

public interface IFileSystemFactory
{
    IFileSystem CreateAggregate(IEnumerable<IFileSystem> fileSystems);
    IFileSystem CreateAssemblyResourceRoot(Assembly assembly);
    IFileSystem CreateAssemblyResourceSystem(Assembly assembly);
    IFileSystem CreateDisk(string basePath);
}