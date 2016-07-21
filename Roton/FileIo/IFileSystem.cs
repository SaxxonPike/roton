using System.Collections.Generic;

namespace Roton.FileIo
{
    public interface IFileSystem
    {
        string GetCombinedPath(params string[] paths);
        IEnumerable<string> GetDirectoryNames(string path);
        byte[] GetFile(string path);
        IEnumerable<string> GetFileNames(string path);
        string GetParentPath(string path);
        void PutFile(string path, byte[] data);
    }
}