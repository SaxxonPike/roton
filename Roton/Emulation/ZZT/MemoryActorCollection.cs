using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.ZZT
{
    internal class MemoryActorCollection : MemoryActorCollectionBase
    {
        public MemoryActorCollection(Memory memory)
            : base(memory)
        {
        }

        public override int Capacity
        {
            get { return 152; }
        }

        public override int Count
        {
            get { return Memory.Read16(0x31CD) + 1; }
        }

        protected override MemoryActor GetActor(int index)
        {
            return new MemoryActor(Memory, 0x31CF + (0x0021 * index));
        }
    }
}
