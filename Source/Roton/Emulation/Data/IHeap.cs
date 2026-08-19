using System;

namespace Roton.Emulation.Data;

public interface IHeap
{
    Memory<char> this[int index] { get; }
    int Size { get; }
    int Allocate(ReadOnlySpan<char> data);
    void FreeAll();
}