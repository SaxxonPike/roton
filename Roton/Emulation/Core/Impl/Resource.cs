using System;

namespace Roton.Emulation.Core.Impl
{
    public class Resource : IResource
    {
        private readonly byte[] _data;

        public Resource(byte[] data)
        {
            if (data == null || data.Length == 0)
                throw new Exception($"Can't resolve resource.");
            _data = data;
        }
        
        public IFileSystem Root => GetPrependedFileSystem("root/");
        public IFileSystem System => GetPrependedFileSystem("system/");

        private IFileSystem GetPrependedFileSystem(string path)
        {
            return new PrependedFileSystem(new ZipFileSystem(_data), path);
        }
    }
}