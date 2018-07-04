using System.Collections.Generic;
using Roton.Core;
using Roton.Core.Collections;

namespace Roton.Emulation.Mapping
{
    public abstract class Actors : FixedList<IActor>, IActors
    {
        protected Actors(IMemory memory, int capacity)
        {
            Memory = memory;
            Capacity = capacity;
        }

        private IDictionary<int, IActor> Cache { get; } = new Dictionary<int, IActor>();

        protected IMemory Memory { get; private set; }

        public int Capacity { get; }

        protected abstract IActor GetActor(int index);

        protected sealed override IActor GetItem(int index)
        {
            IActor actor;
            Cache.TryGetValue(index, out actor);
            if (actor == null)
            {
                actor = GetActor(index);
                Cache[index] = actor;
            }
            return actor;
        }

        protected sealed override void SetItem(int index, IActor value)
        {
            throw Exceptions.InvalidSet;
        }
    }
}