using System;
using System.Buffers;

namespace Roton.Composers.Audio;

public readonly struct AudioComposerDataEventArgs(
    IMemoryOwner<float> memory,
    int length)
{
    public IMemoryOwner<float> Memory => memory;
    public int Length => length;
    public Span<float> Data => memory.Memory.Span.Slice(0, length);
}