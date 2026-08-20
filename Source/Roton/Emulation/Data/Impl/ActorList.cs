using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl;

public abstract class ActorList(IMemory memory, int capacity) : FixedList<IActor>, IActorList
{
    private IActor[] Cache { get; } = new IActor[capacity];

    protected IMemory Memory => memory;

    public int Capacity { get; } = capacity;

    public IActor Player => this[0];

    public IActor ActorAt(Location location) => 
        GetItem(ActorIndexAt(location));

    public int ActorIndexAt(Location location)
    {
        var count = Count;

        for (var i = 0; i < count; i++)
        {
            var actor = GetItem(i);
            if (actor.Location == location)
                return i;
        }

        return -1;
    }

    protected abstract IActor GetActor(int index);

    protected sealed override IActor GetItem(int index)
    {
        if (index < 0 || index >= Capacity)
            return GetActor(index);

        var actor = Cache[index];
        if (actor != null) 
            return actor;
            
        actor = GetActor(index);
        Cache[index] = actor;
        return actor;
    }

    protected sealed override void SetItem(int index, IActor value)
    {
        throw Exceptions.InvalidSet;
    }
}