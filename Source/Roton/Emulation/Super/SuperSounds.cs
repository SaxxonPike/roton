using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperSounds(IMemory memory) : Sounds
{
    public override ISound Forest { get; } = new SuperForestSound(memory, 0x1E5C, 8);
}