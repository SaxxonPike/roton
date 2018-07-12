using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Lyon.App
{
    public interface ILauncher
    {
        void Launch(ContextEngine contextEngine, IFileSystem fileSystem, IConfig config);
    }
}