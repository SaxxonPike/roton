using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalColors(IMemory memory) : Colors(memory, 0xFFF9)
{
    public override int Count => 7;
}