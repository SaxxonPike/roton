using System.Collections.Generic;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl
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

        public IActor Player => this[0];

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