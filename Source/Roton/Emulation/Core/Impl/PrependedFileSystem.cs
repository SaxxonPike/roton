using System.Collections.Generic;

namespace Roton.Emulation.Core.Impl;

public sealed class PrependedFileSystem(IFileSystem baseFileSystem, string basePath) : IFileSystem
{
    public bool IsWriteable => baseFileSystem.IsWriteable;

    public bool FileExists(string path)
        => baseFileSystem.FileExists(basePath + path);

    public byte[] GetFile(string path) 
        => baseFileSystem.GetFile(basePath + path);

    public IEnumerable<string> GetFileNames(string path) 
        => baseFileSystem.GetFileNames(basePath + path);

    public void PutFile(string path, byte[] data) 
        => baseFileSystem.PutFile(basePath + path, data);
}