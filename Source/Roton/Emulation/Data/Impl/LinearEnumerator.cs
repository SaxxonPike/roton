using System;
using System.Collections;
using System.Collections.Generic;

namespace Roton.Emulation.Data.Impl;

public struct LinearEnumerator<T>(Func<int, T> getter, int count) : IEnumerator<T>
{
    private int _index = -1;

    public void Dispose()
    {
    }

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        _index++;
        return _index < count;
    }

    public void Reset()
    {
        _index = -1;
    }

    public T Current => getter(_index);
}