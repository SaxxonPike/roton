using System;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl;

public abstract class Actors(Lazy<IMemory> memory, int capacity) : FixedList<IActor>, IActors
{
    private IActor[] Cache { get; } = new IActor[capacity];

    protected IMemory Memory => memory.Value;

    public int Capacity { get; } = capacity;

    public IActor Player => this[0];

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