using System;
using System.Collections;
using System.Collections.Generic;

namespace Roton.Emulation.Data.Impl;

public struct LinearEnumerator<T>(Func<int, T> getter, int count, int firstIndex) : IEnumerator<T>
{
    private int _index = firstIndex - 1;
    private readonly int _max = count + firstIndex;

    public void Dispose()
    {
    }

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        _index++;
        return _index < _max;
    }

    public void Reset()
    {
        _index = firstIndex - 1;
    }

    public T Current => getter(_index);
}