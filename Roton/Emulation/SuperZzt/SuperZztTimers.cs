using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.SuperZzt
{
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class SuperZztTimers : ITimers
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