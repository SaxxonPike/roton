using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.SuperZZT
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