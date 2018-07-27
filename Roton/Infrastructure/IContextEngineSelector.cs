using Roton.Emulation.Data.Impl;

namespace Roton.Infrastructure
{
    public interface IContextEngineSelector
    {
        Context Get(string filename);
    }
}