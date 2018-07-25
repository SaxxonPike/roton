using Roton.Emulation.Data.Impl;

namespace Roton.Infrastructure
{
    public interface IContextEngineSelector
    {
        ContextEngine Get(string filename);
    }
}