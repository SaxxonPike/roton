using System;

namespace Roton.Emulation.Data.Impl;

public abstract class DrumSoundList(IMemory memory, int offset, int recordSize)
    : IDrumSoundList
{
    public Span<Word> this[int index] =>
        memory.GetSpan<Word>(offset + recordSize * index, recordSize);

    public int Count => 10;
}