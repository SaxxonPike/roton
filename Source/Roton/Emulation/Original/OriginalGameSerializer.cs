using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalGameSerializer(
    IMemory memory)
    : GameSerializer(memory)
{
    public override int WorldDataCapacity => 0x200;
    public override int WorldDataOffset => 0x481E;
    public override int WorldDataSize => 0x0108;
}