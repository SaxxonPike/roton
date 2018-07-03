using System.IO;
using Roton.Core;
using Roton.FileIo;

namespace Lyon.App
{
    public interface ILauncher
    {
        void Launch(ContextEngine contextEngine, IFileSystem fileSystem);
    }
}