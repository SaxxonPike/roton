using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;

namespace Lyon.App
{
    public interface IContextFactory
    {
        IContext Create(ContextEngine contextEngine, IFileSystem fileSystem);
    }
}