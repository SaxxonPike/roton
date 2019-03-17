using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Core
{
    public interface ITracer
    {
        void TraceInput(EngineKeyCode keyCode);
        void TraceOop(IOopContext oopContext);
        void TraceStep();
    }
}