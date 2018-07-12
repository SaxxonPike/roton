using System.Collections.Generic;

namespace Roton.Emulation.Core
{
    public interface IFileSystem
    {
        byte[] GetFile(string path);
        IEnumerable<string> GetFileNames(string path);
        void PutFile(string path, byte[] data);
    }
}