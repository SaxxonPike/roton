using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original
{
    [ContextEngine(ContextEngine.Original)]
    public sealed class OriginalSounds : Sounds
    {
        public override ISound Forest { get; } = new Sound
        (
            0x39, 0x01
        );
    }
}