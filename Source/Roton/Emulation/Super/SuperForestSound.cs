using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Super;

public sealed class SuperForestSound(IMemory memory, int offset, int length) : ISound
{
    public int this[int index] => (index & 1) == 1 ? 0x01 : memory.Read8(offset + (index >> 1));

    public int Length { get; } = length << 1;
}