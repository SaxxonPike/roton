using System;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class ActorList(
    IMemory memory, 
    IFacts facts,
    IFactory factory,
    ICodeHeap heap)
    : CachedFixedList<IActor>(facts.ActorCapacity), IActorList
{
    public int Capacity => facts.ActorCapacity;

    public IActor Player =>
        this[0];

    public int IndexAt(Location location)
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

    public Span<char> GetCode(int index) =>
        heap[GetItem(index).Pointer];

    protected override void SetItem(int index, IActor value) => 
        throw Exceptions.InvalidSet;
    
    public override int Count =>
        memory.GetRef<Word>(facts.ActorDataCountOffset) + 1;

    protected override IActor InitItem(int index) =>
        factory.Create<Actor>(facts.ActorDataOffset + facts.ActorDataLength * index, facts.ActorDataLength);

}