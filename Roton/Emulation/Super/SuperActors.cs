using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super
{
    [ContextEngine(ContextEngine.Super)]
    public sealed class SuperActors : Actors
    {
        public SuperActors(IMemory memory)
            : base(memory, 129)
        {
        }

        public override int Count
            => Memory.Read16(0x6AB3) + 1;

        protected override IActor GetActor(int index)
            => new Actor(Memory, 0x6AB5 + 0x0019 * index);
    }
}