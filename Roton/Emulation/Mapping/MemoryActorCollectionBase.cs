﻿using Roton.Core;

namespace Roton.Emulation.Mapping
{
    internal abstract class MemoryActorCollectionBase : FixedList<IActor>
    {
        protected MemoryActorCollectionBase(IMemory memory)
        {
            Memory = memory;
            Cache = new MemoryActor[Capacity];
            for (var i = 0; i < Capacity; i++)
            {
                Cache[i] = GetActor(i);
            }
        }

        protected override IActor GetItem(int index)
        {
            if (index >= 0 && index < Capacity)
                return Cache[index];
            return GetActor(index);
        }

        protected override void SetItem(int index, IActor value)
        {
            throw Exceptions.InvalidSet;
        }

        private MemoryActor[] Cache { get; }

        public abstract int Capacity { get; }

        protected abstract MemoryActor GetActor(int index);

        public IMemory Memory { get; private set; }
    }
}