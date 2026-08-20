using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperColorList(IMemory memory) : ColorList(memory, 0x21E7)
{
    public override int Count => 7;
}