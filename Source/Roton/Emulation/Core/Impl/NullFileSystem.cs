using System.Collections.Generic;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class NullFileSystem : IFileSystem
{
    public bool IsWriteable => false;

    public bool FileExists(string path) => false;

    public byte[]? GetFile(string path) => null;

    public IEnumerable<string> GetFileNames(string path) => [];

    public void PutFile(string path, byte[] data)
    {
    }
}