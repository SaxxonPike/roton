using System;
using System.IO;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Core
{
    public interface ITracer
    {
        void TraceInput(EngineKeyCode keyCode);
        void TraceOop(ref OopContext oopContext, ref Word instruction);
        void TraceStep();
        void TraceBroadcast(int sender, ReadOnlySpan<char> term, int targetIndex, bool ignoreLock, bool ignoreSelfLock);
        void Attach(TextWriter writer);
        void Detach(TextWriter writer);
    }
}