using System;
using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface ICodeHeap
{
    Span<char> this[int pointer] { get; }
    int Size { get; }
    int Allocate(ReadOnlySpan<char> data);
    void Free(int pointer);
    void FreeAll();
    IReadOnlyList<(int Index, int Pointer)> Compact(IEnumerable<(int Index, int Pointer)> pointers);
}