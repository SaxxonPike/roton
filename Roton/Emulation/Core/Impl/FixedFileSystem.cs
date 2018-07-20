using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Core.Impl
{
    public sealed class FixedFileSystem : IFileSystem
    {
        private readonly IDictionary<string, byte[]> _files;

        public FixedFileSystem(bool writeable, IDictionary<string, byte[]> files = null)
        {
            _files = files ?? new Dictionary<string, byte[]>();
            IsWriteable = writeable;
        }

        public bool IsWriteable { get; }

        public bool FileExists(string path)
        {
            return _files.ContainsKey(path);
        }

        public byte[] GetFile(string path)
        {
            return path == null ? null : _files[path];
        }

        public IEnumerable<string> GetFileNames(string path)
        {
            return _files.Keys;
        }

        public void PutFile(string path, byte[] data)
        {
            _files[path] = data.ToArray();
        }
    }
}
