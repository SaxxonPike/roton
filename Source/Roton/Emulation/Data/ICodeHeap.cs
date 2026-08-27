using System;

namespace Roton.Emulation.Data;

public interface ICodeHeap
{
    Span<char> this[int pointer] { get; }
    int Size { get; }
    int Allocate(ReadOnlySpan<char> data);
    void Free(int pointer);
    void FreeAll();
}