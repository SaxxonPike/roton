using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalColorList(IMemory memory) : ColorList(memory, 0xFFF9)
{
    public override int Count => 7;
}