using Roton.FileIo;

namespace Roton.Core
{
    public interface IResource
    {
        IFileSystem Root { get; }
        IFileSystem System { get; }
    }
}