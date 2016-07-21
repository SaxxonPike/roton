using Roton.Emulation.Execution;

namespace Roton.Emulation.SuperZZT
{
    internal sealed class Sounds : SoundCollectionBase
    {
        public override byte[] Forest => new byte[]
        {
            0x45, 0x01,
            0x40, 0x01,
            0x47, 0x01,
            0x50, 0x01,
            0x46, 0x01,
            0x41, 0x01,
            0x48, 0x01,
            0x50, 0x01
        };
    }
}