using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.SuperZzt
{
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class SuperZztSounds : Sounds
    {
        public SuperZztSounds(IMemory memory)
        {
            Forest = new SuperZztForestSound(memory, 0x1E5C, 8);
        }

        public override ISound Forest { get; }
    }
}