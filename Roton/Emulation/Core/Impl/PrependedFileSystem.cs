using System.Collections.Generic;

namespace Roton.Emulation.Core.Impl
{
    public class PrependedFileSystem : IFileSystem
    {
        private readonly IFileSystem _baseFileSystem;
        private readonly string _basePath;

        public PrependedFileSystem(IFileSystem baseFileSystem, string basePath)
        {
            _baseFileSystem = baseFileSystem;
            _basePath = basePath;
        }

        public byte[] GetFile(string path)
        {
            return _baseFileSystem.GetFile($"{_basePath}{path}");
        }

        public IEnumerable<string> GetFileNames(string path)
        {
            return _baseFileSystem.GetFileNames($"{_basePath}{path}");
        }

        public void PutFile(string path, byte[] data)
        {
            _baseFileSystem.PutFile($"{_basePath}{path}", data);
        }
    }
}
