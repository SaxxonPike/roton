using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Emulation.SuperZZT
{
    public sealed class SuperZztSounds : Sounds
    {
        public SuperZztSounds(IMemory memory)
        {
            Forest = new SuperZztForestSound(memory, 0x1E5C, 8);
        }

        public override ISound Forest { get; }
    }
}