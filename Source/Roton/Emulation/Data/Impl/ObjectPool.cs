using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Roton.Emulation.Data.Impl;

public abstract class ObjectPool<T>(Func<T> factory, Action<T> reset)
{
    private readonly ThreadLocal<Stack<T>> _pool = new(() => new Stack<T>());

    public T Rent() => 
        _pool.Value.Count == 0 ? factory() : _pool.Value.Pop();

    public void Return(T item)
    {
        reset(item);
        _pool.Value.Push(item);
    }
}