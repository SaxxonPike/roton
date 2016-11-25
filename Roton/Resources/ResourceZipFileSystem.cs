using System.Collections.Generic;
using Roton.FileIo;

namespace Roton.Resources
{
    public static class ResourceZipFileSystem
    {
        public static IFileSystem Root => GetPrependedFileSystem("root/");
        public static IFileSystem System => GetPrependedFileSystem("system/");

        private static IFileSystem GetPrependedFileSystem(string path)
        {
            return new PrependedFileSystem(new ZipFileSystem(Properties.Resources.resources), path);
        }
    }
}