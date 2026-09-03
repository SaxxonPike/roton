using System;
using Roton.Infrastructure;

namespace Roton.Emulation.Core;

public interface IMusicEncoder
{
    TempMemory<byte> Encode(ReadOnlySpan<char> music);
}