using System;
using System.Collections;
using System.Collections.Generic;

namespace Roton.Emulation.Data.Impl;

/// <summary>
/// Base class for lists of items that have a fixed length internally.
/// </summary>
/// <typeparam name="T">
/// Type of items in the list.
/// </typeparam>
public abstract class FixedList<T> : IList<T>, IReadOnlyList<T>
{
    private Func<int, T> _getter;

    /// <summary>
    /// Index of the first item in the list. Used for enumeration.
    /// </summary>
    protected virtual int FirstIndex => 0;

    public virtual void Add(T item)
    {
        throw new InvalidOperationException();
    }

    public virtual void Clear()
    {
        throw new InvalidOperationException();
    }

    public virtual bool Contains(T item)
    {
        for (var i = 0; i < Count; i++)
            if (EqualsItem(i + FirstIndex, item))
                return true;

        return false;
    }

    public virtual void CopyTo(T[] array, int arrayIndex)
    {
        throw new InvalidOperationException();
    }

    public abstract int Count { get; }

    public virtual bool IsReadOnly => false;

    public virtual bool Remove(T item)
    {
        throw new InvalidOperationException();
    }

    IEnumerator IEnumerable.GetEnumerator() => 
        GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => 
        GetEnumerator();

    public LinearEnumerator<T> GetEnumerator()
    {
        _getter ??= GetItem;
        return new LinearEnumerator<T>(_getter, Count, FirstIndex);
    }

    public virtual int IndexOf(T item)
    {
        return -1;
    }

    public virtual void Insert(int index, T item)
    {
        throw new InvalidOperationException();
    }

    public T this[int index]
    {
        get => GetItem(index);
        set => SetItem(index, value);
    }

    public virtual void RemoveAt(int index)
    {
        throw new InvalidOperationException();
    }

    protected virtual T GetItem(int index)
    {
        throw new InvalidOperationException();
    }

    protected virtual void SetItem(int index, T value)
    {
    }

    protected virtual bool EqualsItem(int index, T value) =>
        GetItem(index).GetHashCode() == value.GetHashCode();
}