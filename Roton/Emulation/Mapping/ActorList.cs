using Roton.Core;
using Roton.Core.Collections;

namespace Roton.Emulation.Mapping
{
    internal abstract class ActorList : FixedList<IActor>, IActorList
    {
        protected ActorList(IMemory memory, int capacity)
        {
            Memory = memory;
            Capacity = capacity;
            Cache = new IActor[capacity];
            for (var i = 0; i < capacity; i++)
            {
                Cache[i] = GetActor(i);
            }
        }

        private IActor[] Cache { get; }

        protected IMemory Memory { get; private set; }

        public int Capacity { get; }

        protected abstract IActor GetActor(int index);

        protected sealed override IActor GetItem(int index)
        {
            if (index >= 0 && index < Capacity)
                return Cache[index];
            return GetActor(index);
        }

        protected sealed override void SetItem(int index, IActor value)
        {
            throw Exceptions.InvalidSet;
        }
    }
}