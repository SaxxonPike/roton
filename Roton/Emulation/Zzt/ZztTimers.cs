using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Zzt
{
    public class ZztTimers : ITimers
    {
        public ZztTimers(IMemory memory)
        {
            Player = new MemoryTimer(memory, 0x740A);
            TimeLimit = new MemoryTimer(memory, 0x4920);
        }

        public ITimer Player { get; }
        public ITimer TimeLimit { get; }
    }
}