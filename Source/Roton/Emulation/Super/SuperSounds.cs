using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public sealed class SuperSounds(IMemory memory) : Sounds
{
    public override ISound Forest { get; } = new SuperForestSound(memory, 0x1E5C, 8);
}