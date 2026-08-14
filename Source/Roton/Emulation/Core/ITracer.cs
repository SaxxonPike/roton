using System.IO;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Core
{
    public interface ITracer
    {
        void TraceInput(EngineKeyCode keyCode);
        void TraceOop(IOopContext oopContext);
        void TraceStep();
        void TraceBroadcast(int sender, string term, int targetIndex, bool ignoreLock, bool ignoreSelfLock);
        void Attach(TextWriter writer);
        void Detach(TextWriter writer);
    }
}