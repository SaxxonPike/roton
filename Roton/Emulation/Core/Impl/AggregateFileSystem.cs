using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Core.Impl
{
    public sealed class AggregateFileSystem : IFileSystem
    {
        private readonly IEnumerable<IFileSystem> _fileSystems;

        public AggregateFileSystem(IEnumerable<IFileSystem> fileSystems)
        {
            _fileSystems = fileSystems;
        }

        private IFileSystem FindPath(string path, bool isWriteable)
        {
            return _fileSystems
                .FirstOrDefault(fs => fs.FileExists(path) && (!isWriteable || fs.IsWriteable));
        }

        public bool IsWriteable 
            => _fileSystems.Any(fs => fs.IsWriteable);

        public bool FileExists(string path) => FindPath(path, false) != null;

        public byte[] GetFile(string path) => FindPath(path, false)?.GetFile(path);

        public IEnumerable<string> GetFileNames(string path) => FindPath(path, false)?.GetFileNames(path);

        public void PutFile(string path, byte[] data) => 
            (FindPath(path, true) ?? _fileSystems.FirstOrDefault(fs => fs.IsWriteable))?.PutFile(path, data);
    }
}