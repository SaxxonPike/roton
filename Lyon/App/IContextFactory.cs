using Roton.Core;
using Roton.FileIo;

namespace Lyon.App
{
    public interface IContextFactory
    {
        IContext Create(ContextEngine contextEngine, IFileSystem fileSystem);
    }
}