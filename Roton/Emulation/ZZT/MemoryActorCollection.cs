using Roton.Core;
using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Emulation.ZZT
{
    internal sealed class MemoryActorCollection : MemoryActorCollectionBase
    {
        public MemoryActorCollection(IMemory memory)
            : base(memory)
        {
        }

        public override int Capacity => 152;

        public override int Count => Memory.Read16(0x31CD) + 1;

        protected override MemoryActor GetActor(int index)
        {
            return new MemoryActor(Memory, 0x31CF + 0x0021*index);
        }
    }
}