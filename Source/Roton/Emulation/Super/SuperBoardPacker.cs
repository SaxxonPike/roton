using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperBoardPacker(
    IMemory memory,
    ICodeHeap heap,
    ITiles tiles)
    : BoardPacker(memory, heap, tiles)
{
    public override int ActorCapacity => 129;
    public override int ActorDataCountOffset => 0x6AB3;
    public override int ActorDataLength => 0x19;
    public override int ActorDataOffset => 0x6AB5;
    public override int BoardDataLength => 0x1C;
    public override int BoardDataOffset => 0x7767;
    public override int BoardNameLength => 0x3D;
    public override int BoardNameOffset => 0x2BAE;
}