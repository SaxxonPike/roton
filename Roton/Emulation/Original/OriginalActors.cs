using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [ContextEngine(ContextEngine.Original)]
    public sealed class OriginalActors : Actors
    {
        private readonly IHeap _heap;

        public OriginalActors(IMemory memory, IHeap heap)
            : base(memory, 152)
        {
            _heap = heap;
        }

        public override int Count => Memory.Read16(0x31CD) + 1;

        protected override IActor GetActor(int index)
        {
            return new Actor(Memory, _heap, 0x31CF + 0x0021 * index);
        }
    }
}