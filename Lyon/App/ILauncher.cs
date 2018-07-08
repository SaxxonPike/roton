using Roton.Core;
using Roton.Emulation.Data.Impl;
using Roton.FileIo;

namespace Lyon.App
{
    public interface ILauncher
    {
        void Launch(ContextEngine contextEngine, IFileSystem fileSystem);
    }
}