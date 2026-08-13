using System.Collections.Generic;

namespace Roton.Emulation.Data.Impl;

internal static class EnumerableExtensions
{
    public static void CopyTo<T>(this IEnumerable<T> enumarable, IList<T> target)
    {
        var i = 0;
        foreach (var e in enumarable)
            target[i++] = e;
    }
}