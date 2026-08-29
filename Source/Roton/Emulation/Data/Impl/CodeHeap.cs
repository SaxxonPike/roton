using System;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class CodeHeap : ICodeHeap
{
    private int _nextEntry;
    private readonly char[] _block = new char[short.MaxValue - 1];

    public int Size => _nextEntry;

    public int Allocate(ReadOnlySpan<char> data)
    {
        if (data.Length == 0)
            return 0;

        var offset = _nextEntry;
        _nextEntry += data.Length;

        data.CopyTo(_block.AsSpan(offset, data.Length));
        var result = offset | (data.Length << 16);

        return result;
    }

    private Span<char> GetSpan(int pointer)
    {
        var length = unchecked((short)(pointer >> 16));
        var offset = unchecked((short)pointer);
        
        if (length < 0 || offset < 0)
            return Span<char>.Empty;
        
        return _block.AsSpan(offset, length);
    }

    public void Free(int pointer) => 
        GetSpan(pointer).Clear();

    public void FreeAll()
    {
        _block.AsSpan().Clear();
        _nextEntry = 0;
    }

    public Span<char> this[int pointer] =>
        GetSpan(pointer);
}