using System;
using System.Collections.Generic;

namespace Roton.Emulation
{
    public sealed class LinearEnumerator<T> : IEnumerator<T>
    {
        public LinearEnumerator(Func<int, T> getter, int count)
        {
            Count = count;
            Getter = getter;
            Reset();
        }

        private int Count { get; }

        public T Current => Getter(Index);

        object System.Collections.IEnumerator.Current => Getter(Index);

        public void Dispose()
        {
            Getter = null;
        }

        private Func<int, T> Getter { get; set; }

        private int Index { get; set; }

        public bool MoveNext()
        {
            Index++;
            return Index < Count;
        }

        public void Reset()
        {
            Index = -1;
        }
    }
}