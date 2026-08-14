using Roton.Emulation.Core;

namespace Lyon.App;

public interface IFileSystemFactory
{
    IFileSystem Create(string path);
}