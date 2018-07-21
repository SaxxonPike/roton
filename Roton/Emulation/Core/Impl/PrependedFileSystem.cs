using System.Collections.Generic;

namespace Roton.Emulation.Core.Impl
{
    public sealed class PrependedFileSystem : IFileSystem
    {
        private readonly IFileSystem _baseFileSystem;
        private readonly string _basePath;

        public PrependedFileSystem(IFileSystem baseFileSystem, string basePath)
        {
            _baseFileSystem = baseFileSystem;
            _basePath = basePath;
        }

        public bool IsWriteable => _baseFileSystem.IsWriteable;

        public bool FileExists(string path)
            => _baseFileSystem.FileExists($"{_basePath}{path}");

        public byte[] GetFile(string path) 
            => _baseFileSystem.GetFile($"{_basePath}{path}");

        public IEnumerable<string> GetFileNames(string path) 
            => _baseFileSystem.GetFileNames($"{_basePath}{path}");

        public void PutFile(string path, byte[] data) 
            => _baseFileSystem.PutFile($"{_basePath}{path}", data);
    }
}
