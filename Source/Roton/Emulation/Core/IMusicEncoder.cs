using System;
using Roton.Infrastructure;

namespace Roton.Emulation.Core;

public interface IMusicEncoder
{
    /// <remarks>
    /// RoZ: SoundParse
    /// </remarks>
    TempMemory<byte> Encode(ReadOnlySpan<char> music);
}