using System;

namespace Roton.Emulation.Data;

public interface IDrumSoundList
{
    Span<Word> this[int index] { get; }
    int Count { get; }
}