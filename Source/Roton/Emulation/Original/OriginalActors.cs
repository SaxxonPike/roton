using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalActors(IMemory memory, IHeap heap) : Actors(memory, 152)
{
    public override int Count => Memory.GetRef<Word>(0x31CD) + 1;

    private IHeap Heap => heap;

    protected override IActor GetActor(int index)
    {
        return new Actor(Memory, Heap, 0x31CF + 0x0021 * index);
    }
}