using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super
{
    [Context(Context.Super)]
    public sealed class SuperActors : Actors
    {
        private readonly Lazy<IHeap> _heap;

        public SuperActors(Lazy<IMemory> memory, Lazy<IHeap> heap)
            : base(memory, 129)
        {
            _heap = heap;
        }

        private IHeap Heap => _heap.Value;

        public override int Count
            => Memory.Read16(0x6AB3) + 1;

        protected override IActor GetActor(int index)
            => new Actor(Memory, Heap, 0x6AB5 + 0x0019 * index);
    }
}