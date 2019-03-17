using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [Context(Context.Original)]
    public sealed class OriginalTimers : ITimers
    {
        public OriginalTimers(IMemory memory)
        {
            Player = new MemoryTimer(memory, 0x740A);
            TimeLimit = new MemoryTimer(memory, 0x4920);
        }

        public ITimer Player { get; }
        public ITimer TimeLimit { get; }
    }
}