using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Super;

public sealed class SuperForestSound : ISound
{
    private readonly IMemory _memory;
    private readonly int _offset;

    public SuperForestSound(IMemory memory, int offset, int length)
    {
        _memory = memory;
        _offset = offset;
        Length = length << 1;
    }

    public int this[int index] => (index & 1) == 1 ? 0x01 : _memory.Read8(_offset + (index >> 1));

    public int Length { get; }
}