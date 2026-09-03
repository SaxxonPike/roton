using System;

namespace Roton;

internal static class SpanExtensions
{
    public static void TryCopyTo<T>(this ReadOnlySpan<T> source, Span<T> destination)
    {
        var len = Math.Min(source.Length, destination.Length);
        source.Slice(0, len).CopyTo(destination);
    }

    public static void TryCopyTo<T>(this Span<T> source, Span<T> destination)
    {
        var len = Math.Min(source.Length, destination.Length);
        source.Slice(0, len).CopyTo(destination);
    }
}