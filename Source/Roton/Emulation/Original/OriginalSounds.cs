using System;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalSounds : Sounds
{
    private readonly byte[] _forest = CreateSound
    (
        0x39, 0x01
    );

    public override ReadOnlySpan<byte> Forest => _forest;
}