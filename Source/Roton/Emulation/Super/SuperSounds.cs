using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperSounds(IMemory memory) : Sounds
{
    private readonly byte[] _forest = CreateSound(
        memory.Data[0x1E5C], 0x01,
        memory.Data[0x1E5D], 0x01,
        memory.Data[0x1E5E], 0x01,
        memory.Data[0x1E5F], 0x01,
        memory.Data[0x1E60], 0x01,
        memory.Data[0x1E61], 0x01,
        memory.Data[0x1E62], 0x01,
        memory.Data[0x1E63], 0x01
    );

    public override ReadOnlySpan<byte> Forest => _forest;
}