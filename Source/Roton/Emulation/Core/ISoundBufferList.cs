using System;
using System.Collections.Generic;

namespace Roton.Emulation.Core;

public interface ISoundBufferList : IList<int>
{
    void Enqueue(ReadOnlySpan<byte> sound);
    SoundNote Dequeue();
}