using System;

namespace Roton.Emulation
{
    internal abstract partial class MemoryActorCollectionBase : FixedList<Actor>
    {
        public MemoryActorCollectionBase(Memory memory)
        {
            Memory = memory;
            Cache = new MemoryActor[Capacity];
            for (var i = 0; i < Capacity; i++)
            {
                Cache[i] = GetActor(i);
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
            set { throw new NotImplementedException(); }
        }

        private MemoryActor[] Cache { get; }

        public abstract int Capacity { get; }

        protected abstract MemoryActor GetActor(int index);

        public Memory Memory { get; private set; }
    }
}