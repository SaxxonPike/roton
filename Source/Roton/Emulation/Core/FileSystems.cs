using System.Collections.Generic;
using Roton.Emulation.Core.Impl;

namespace Roton.Emulation.Core;

public static class FileSystems
{
    public static IFileSystem Aggregate(IEnumerable<IFileSystem> fileSystems) =>
        new AggregateFileSystem(fileSystems);

    public static IFileSystem Disk(string basePath) =>
        new DiskFileSystem(basePath);
}