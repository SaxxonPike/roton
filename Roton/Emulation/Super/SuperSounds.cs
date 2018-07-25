using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super
{
    [ContextEngine(ContextEngine.Super)]
    public sealed class SuperSounds : Sounds
    {
        public SuperSounds(IMemory memory)
        {
            Forest = new SuperForestSound(memory, 0x1E5C, 8);
        }

        public override ISound Forest { get; }
    }
}