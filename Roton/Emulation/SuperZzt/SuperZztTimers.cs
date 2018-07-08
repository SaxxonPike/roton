using Roton.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Execution;

namespace Roton.Emulation.SuperZZT
{
    public class SuperZztTimers : ITimers
    {
        public SuperZztTimers(IMemory memory)
        {
            Player = new MemoryTimer(memory, 0xB95E);
            TimeLimit = new MemoryTimer(memory, 0x79CA);
        }

        public ITimer Player { get; }
        public ITimer TimeLimit { get; }
    }
}