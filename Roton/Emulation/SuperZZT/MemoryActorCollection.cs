using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.SuperZZT
{
    internal class MemoryActorCollection : MemoryActorCollectionBase
    {
        public MemoryActorCollection(Memory memory)
            : base(memory)
        {
        }

        public override int Capacity
        {
            get { return 129; }
        }
        
        public override int Count
        {
            get { return Memory.Read16(0x6AB3) + 1; }
        }

        protected override MemoryActor GetActor(int index)
        {
            return new MemoryActor(Memory, 0x6AB5 + (0x0019 * index));
        }
    }
}
