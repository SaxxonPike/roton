using Roton.Emulation.Data;

namespace Roton.Emulation.Core
{
    public interface ITracer
    {
        void Trace(IOopContext oopContext);
    }
}