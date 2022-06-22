using System.Collections.Generic;

namespace Roton.Emulation.Data.Impl;

internal static class ArrayExtensions
{
    public static void CopyTo<T>(this T[] array, IList<T> target)
    {
        for (var i = 0; i < array.Length; i++)
            target[i] = array[i];
    }
}