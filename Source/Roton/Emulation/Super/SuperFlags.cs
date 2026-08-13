using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperFlags : Flags
{
    public SuperFlags(IMemory memory)
        : base(memory, 0x7863 + 21)
    {
    }

    public override int Count => 16;
}