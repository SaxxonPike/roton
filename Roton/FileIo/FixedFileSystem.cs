using System.Collections.Generic;
using System.Linq;

namespace Roton.FileIo
{
    public class FixedFileSystem : IFileSystem
    {
        private readonly IDictionary<string, byte[]> _files;

        public FixedFileSystem(IDictionary<string, byte[]> files = null)
        {
            _files = files ?? new Dictionary<string, byte[]>();
        }

        public byte[] GetFile(string path)
        {
            return _files.ContainsKey(path) ? _files[path] : null;
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
