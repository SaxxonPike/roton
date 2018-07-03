using System.Collections.Generic;

namespace Roton.FileIo
{
    public interface IFileSystem
    {
        byte[] GetFile(string path);
        IEnumerable<string> GetFileNames(string path);
        void PutFile(string path, byte[] data);
    }
}