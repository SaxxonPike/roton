using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperActorList(IMemory memory, ICodeHeap heap) : ActorList(memory, 129)
{
    private ICodeHeap Heap => heap;

    public override int Count
        => Memory.GetRef<Word>(0x6AB3) + 1;

    protected override IActor InitItem(int index)
        => new Actor(Memory, Heap, 0x6AB5 + 0x0019 * index);
}