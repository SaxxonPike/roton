using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    internal class MemoryActorCollection : MemoryActorCollectionBase
    {
        public MemoryActorCollection(IMemory memory)
            : base(memory)
        {
        }

        public override int Capacity => 129;

        public override int Count => Memory.Read16(0x6AB3) + 1;

        protected override MemoryActor GetActor(int index)
        {
            return new MemoryActor(Memory, 0x6AB5 + 0x0019*index);
        }
    }
}