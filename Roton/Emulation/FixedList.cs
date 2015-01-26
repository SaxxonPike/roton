using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    abstract public class FixedList<T> : IList<T>
    {
        virtual public int IndexOf(T item)
        {
            return -1;
        }

        virtual public void Insert(int index, T item)
        {
            throw new InvalidOperationException();
        }

        virtual public void RemoveAt(int index)
        {
            throw new InvalidOperationException();
        }

        abstract public T this[int index]
        {
            get;
            set;
        }

        virtual public void Add(T item)
        {
            throw new InvalidOperationException();
        }

        virtual public void Clear()
        {
            throw new InvalidOperationException();
        }

        virtual public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        virtual public void CopyTo(T[] array, int arrayIndex)
        {
            throw new InvalidOperationException();
        }

        abstract public int Count
        {
            get;
        }

        virtual public bool IsReadOnly
        {
            get { return false; }
        }

        virtual public bool Remove(T item)
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
            return (IList<T>)this;
        }
    }
}
