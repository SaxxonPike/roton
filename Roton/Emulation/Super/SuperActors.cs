using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super
{
    [ContextEngine(ContextEngine.Super)]
    public sealed class SuperActors : Actors
    {
        private readonly IHeap _heap;

        public SuperActors(IMemory memory, IHeap heap)
            : base(memory, 129)
        {
            _heap = heap;
        }

        public override int Count
            => Memory.Read16(0x6AB3) + 1;

        protected override IActor GetActor(int index)
            => new Actor(Memory, _heap, 0x6AB5 + 0x0019 * index);
    }
}