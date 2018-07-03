using System.IO;
using Roton.Core;
using Roton.FileIo;

namespace Lyon.App
{
    public class ContextFactory : IContextFactory
    {
        public IContext Create(string fileName)
        {
            return new Context();
        }

        public IContext Create(Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public IContext Create(ContextEngine contextEngine, IFileSystem fileSystem)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IContextFactory
    {
        IContext Create(string fileName);
        IContext Create(Stream stream);
        IContext Create(ContextEngine contextEngine, IFileSystem fileSystem);
    }
}