using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperGameSerializer(
    IMemory memory)
    : GameSerializer(memory)
{
    public override int WorldDataCapacity => 0x400;
    public override int WorldDataOffset => 0x784C;
    public override int WorldDataSize => 0x0187;
}