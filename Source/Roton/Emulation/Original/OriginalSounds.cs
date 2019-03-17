using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [Context(Context.Original)]
    public sealed class OriginalSounds : Sounds
    {
        public override ISound Forest { get; } = new Sound
        (
            0x39, 0x01
        );
    }
}