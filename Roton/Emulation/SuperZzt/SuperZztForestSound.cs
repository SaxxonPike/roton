using Roton.Emulation.Data;

namespace Roton.Emulation.SuperZzt
{
    public sealed class SuperZztForestSound : ISound
    {
        private readonly IMemory _memory;
        private readonly int _offset;

        public SuperZztForestSound(IMemory memory, int offset, int length)
        {
            _memory = memory;
            _offset = offset;
            Length = length << 1;
        }

        public int this[int index] => (index & 1) == 1 ? 0x01 : _memory.Read8(_offset + (index >> 1));

        public int Length { get; }
    }
}