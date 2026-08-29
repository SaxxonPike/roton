namespace Roton.Emulation.Core.Impl;

internal sealed class Resource : IResource
{
    private readonly byte[] _data;

    public Resource(byte[] data)
    {
        if (data == null || data.Length == 0)
            throw new RotonException("Can't resolve resource.");
        _data = data;
    }
        
    public IFileSystem Root => GetPrependedFileSystem("root/");
    public IFileSystem System => GetPrependedFileSystem("system/");

    private PrependedFileSystem GetPrependedFileSystem(string path) => 
        new PrependedFileSystem(new ZipFileSystem(_data), path);
}