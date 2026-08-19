using System.Collections.Generic;

namespace Roton.Emulation.Core.Impl;

public sealed class FixedFileSystem(bool writeable, IDictionary<string, byte[]>? files = null) : IFileSystem
{
    private readonly IDictionary<string, byte[]> _files = files ?? new Dictionary<string, byte[]>();

    public bool IsWriteable { get; } = writeable;

    public bool FileExists(string path)
    {
        return _files.ContainsKey(path);
    }

    public byte[]? GetFile(string? path)
    {
        return path == null ? null : _files[path];
    }

    public IEnumerable<string> GetFileNames(string path)
    {
        return _files.Keys;
    }

    public void PutFile(string path, byte[] data)
    {
        _files[path] = [.. data];
    }
}