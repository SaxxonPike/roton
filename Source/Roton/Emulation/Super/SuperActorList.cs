using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperActorList(
    IMemory memory,
    ICodeHeap heap)
    : ActorList(memory, 129)
{
    public override int Count =>
        Memory.GetRef<Word>(0x6AB3) + 1;

    protected override IActor InitItem(int index) =>
        new Actor(Memory, heap, 0x6AB5 + 0x0019 * index);

    public override Span<char> GetActorCode(int index) =>
        heap[GetItem(index).Pointer];
}