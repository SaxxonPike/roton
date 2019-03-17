using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super
{
    [Context(Context.Super)]
    public sealed class SuperTimers : ITimers
    {
        public SuperTimers(IMemory memory)
        {
            Player = new MemoryTimer(memory, 0xB95E);
            TimeLimit = new MemoryTimer(memory, 0x79CA);
        }

        public ITimer Player { get; }
        public ITimer TimeLimit { get; }
    }
}