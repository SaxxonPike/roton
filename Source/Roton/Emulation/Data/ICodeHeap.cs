using System;

namespace Roton.Emulation.Data;

public interface ICodeHeap
{
    Memory<char> this[int index] { get; }
    int Size { get; }
    int Allocate(ReadOnlySpan<char> data);
    void Free(int index);
    void FreeAll();
}