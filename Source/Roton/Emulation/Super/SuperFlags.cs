using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperFlags(IMemory memory) : Flags(memory, 0x7863 + 21)
{
    public override int Count => 16;

    protected override int ItemLength => 21;
}