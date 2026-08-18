using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IMusicEncoder
{
    ISound Encode(ReadOnlySpan<char> music);
}