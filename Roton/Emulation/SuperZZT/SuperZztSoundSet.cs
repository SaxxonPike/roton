using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class SuperZztSoundSet : SoundSet
    {
        public SuperZztSoundSet(IMemory memory)
        {
            Forest = new SuperZztForestSound(memory, 0x1E5C, 8);
        }

        public override ISound Forest { get; }
    }
}