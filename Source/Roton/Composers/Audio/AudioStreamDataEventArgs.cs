using System;
using Roton.Infrastructure;

namespace Roton.Composers.Audio;

public readonly struct AudioStreamDataEventArgs(TempMemory<float> memory, int length)
{
    public TempMemory<float> Memory => memory;
    public Span<float> Data => memory.Span.Slice(0, length);
}