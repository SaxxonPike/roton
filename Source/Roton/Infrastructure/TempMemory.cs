using System;
using System.Buffers;
using JetBrains.Annotations;

namespace Roton.Infrastructure;

[MustDisposeResource]
public readonly struct TempMemory<T>(int length) : IDisposable
{
    public TempMemory(ReadOnlySpan<T> source) : this(source.Length) => 
        source.CopyTo(Raw);

    public T[] Raw { get; } = 
        Rent(length);

    public Span<T> Span =>
        Raw.AsSpan(0, length);

    public void Dispose() =>
        ArrayPool<T>.Shared.Return(Raw);

    private static T[] Rent(int length)
    {
        var arr = ArrayPool<T>.Shared.Rent(length);
        return arr;
    }
}