using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalActorList(IMemory memory, ICodeHeap heap) : ActorList(memory, 152)
{
    public override int Count => Memory.GetRef<Word>(0x31CD) + 1;

    private ICodeHeap Heap => heap;

    protected override IActor InitItem(int index)
    {
        return new Actor(Memory, Heap, 0x31CF + 0x0021 * index);
    }
}