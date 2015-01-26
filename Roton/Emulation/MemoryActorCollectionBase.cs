using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    abstract internal partial class MemoryActorCollectionBase : FixedList<Actor>
    {
        public MemoryActorCollectionBase(Memory memory)
        {
            this.Memory = memory;
            this.Cache = new MemoryActor[Capacity];
            for (int i = 0; i < Capacity; i++)
            {
                this.Cache[i] = GetActor(i);
            }
        }

        public override Actor this[int index]
        {
            get
            {
                if (index >= 0 && index < Capacity)
                    return Cache[index];
                return GetActor(index);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private MemoryActor[] Cache
        {
            get;
            set;
        }

        abstract public int Capacity { get; }

        abstract protected MemoryActor GetActor(int index);

        public Memory Memory
        {
            get;
            private set;
        }
    }
}
