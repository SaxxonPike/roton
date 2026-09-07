using System;

namespace Roton.Emulation.Core;

public interface IRandomizer
{
    void Reset();
    void SetSeed(DateTime now);
    int GetNext(int exclusiveUpperBound);
    void GetNext(int exclusiveUpperBound, Span<int> buffer);
    int State { get; set; }
}