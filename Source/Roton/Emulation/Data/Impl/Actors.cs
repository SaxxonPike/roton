using System;
using System.Collections.Generic;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Data.Impl;

public abstract class Actors : FixedList<IActor>, IActors
{
    protected Actors(Lazy<IMemory> memory, int capacity)
    {
        _memory = memory;
        Capacity = capacity;
    }

    private IDictionary<int, IActor> Cache { get; } = new Dictionary<int, IActor>();

    private readonly Lazy<IMemory> _memory;

    protected IMemory Memory => _memory.Value;

    public int Capacity { get; }

    public IActor Player => this[0];

    protected abstract IActor GetActor(int index);

    protected sealed override IActor GetItem(int index)
    {
        Cache.TryGetValue(index, out var actor);
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