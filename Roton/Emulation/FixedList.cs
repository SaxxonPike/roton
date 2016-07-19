using System;
using System.Collections.Generic;

namespace Roton.Emulation
{
    public abstract class FixedList<T> : IList<T>
    {
        public virtual int IndexOf(T item)
        {
            return -1;
        }

        public virtual void Insert(int index, T item)
        {
            throw new InvalidOperationException();
        }

        public virtual void RemoveAt(int index)
        {
            throw new InvalidOperationException();
        }

        public abstract T this[int index] { get; set; }

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
            return IndexOf(item) >= 0;
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

        public IEnumerator<T> GetEnumerator()
        {
            return new LinearEnumerator<T>(GetItem, Count);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new LinearEnumerator<T>(GetItem, Count);
        }

        private T GetItem(int index)
        {
            return this[index];
        }

        public IList<T> AsList()
        {
            return (IList<T>) this;
        }
    }
}