using System.Collections.Generic;
using System.Linq;

namespace Roton.Emulation.Core.Impl
{
    public sealed class FixedFileSystem : IFileSystem
    {
        private readonly IFileSystem _fallbackFileSystem;
        private readonly IDictionary<string, byte[]> _files;

        public FixedFileSystem(IDictionary<string, byte[]> files = null, IFileSystem fallback = null)
        {
            _fallbackFileSystem = fallback;
            _files = files ?? new Dictionary<string, byte[]>();
        }

        public byte[] GetFile(string path)
        {
            if (path == null)
                return null;
            
            return _files.ContainsKey(path) ? _files[path] : _fallbackFileSystem.GetFile(path);
        }

        public IEnumerable<string> GetFileNames(string path)
        {
            return _files.Keys.Concat(_fallbackFileSystem?.GetFileNames(path) ?? Enumerable.Empty<string>());
        }

        public void PutFile(string path, byte[] data)
        {
            _files[path] = data.ToArray();
            _fallbackFileSystem.PutFile(path, data);
        }
    }
}
