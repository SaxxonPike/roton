using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Zzt
{
    [ContextEngine(ContextEngine.Zzt)]
    public sealed class ZztSounds : Sounds
    {
        public override ISound Forest { get; } = new Sound
        (
            0x39, 0x01
        );
    }
}