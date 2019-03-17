using System;
using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Core.Impl
{
    public static class FileSystemExtensions
    {
        public static IEnumerable<string> GetFileNames(this IFileSystem fileSystem, string path, string extension)
        {
            return fileSystem
                .GetFileNames(path)
                .Where(f => f.EndsWith(extension, StringComparison.OrdinalIgnoreCase));
        }
    }
}