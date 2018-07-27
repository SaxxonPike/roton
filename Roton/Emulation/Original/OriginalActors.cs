using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [Context(Context.Original)]
    public sealed class OriginalActors : Actors
    {
        private readonly Lazy<IHeap> _heap;

        public OriginalActors(Lazy<IMemory> memory, Lazy<IHeap> heap)
            : base(memory, 152)
        {
            _heap = heap;
        }

        public override int Count => Memory.Read16(0x31CD) + 1;

        private IHeap Heap => _heap.Value;

        protected override IActor GetActor(int index)
        {
            return new Actor(Memory, Heap, 0x31CF + 0x0021 * index);
        }
    }
}