using System;
using System.Collections;
using System.Collections.Generic;

namespace Roton.Core.Collections
{
    internal sealed class LinearEnumerator<T> : IEnumerator<T>
    {
        public LinearEnumerator(Func<int, T> getter, int count)
        {
            Count = count;
            Getter = getter;
            Reset();
        }

        private int Count { get; }

        private Func<int, T> Getter { get; set; }

        private int Index { get; set; }

        public void Dispose()
        {
            Getter = null;
        }

        object IEnumerator.Current => Getter(Index);

        public bool MoveNext()
        {
            Index++;
            return Index < Count;
        }

        public void Reset()
        {
            Index = -1;
        }

        public T Current => Getter(Index);
    }
}