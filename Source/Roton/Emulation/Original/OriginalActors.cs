using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalActors(Lazy<IMemory> memory, Lazy<IHeap> heap) : Actors(memory, 152)
{
    public override int Count => Memory.Read16(0x31CD) + 1;

    private IHeap Heap => heap.Value;

    protected override IActor GetActor(int index)
    {
        return new Actor(Memory, Heap, 0x31CF + 0x0021 * index);
    }
}