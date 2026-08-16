using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Roton.Emulation.Data.Impl;

public abstract class ObjectPool<T>(Func<T> factory, Action<T> reset)
{
    private readonly ConcurrentStack<T> _pool = new();

    public T Rent() =>
        _pool.TryPop(out var item) ? item : factory();

    public void Return(T item)
    {
        reset(item);
        _pool.Push(item);
    }
}