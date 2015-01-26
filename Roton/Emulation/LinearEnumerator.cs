using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    sealed public class LinearEnumerator<T> : IEnumerator<T>
    {
        public LinearEnumerator(Func<int, T> getter, int count)
        {
            this.Count = count;
            this.Getter = getter;
            Reset();
        }

        private int Count
        {
            get;
            set;
        }

        public T Current
        {
            get { return Getter(Index); }
        }

        object System.Collections.IEnumerator.Current
        {
            get { return Getter(Index); }
        }

        public void Dispose()
        {
            this.Getter = null;
        }

        private Func<int, T> Getter
        {
            get;
            set;
        }

        private int Index
        {
            get;
            set;
        }

        public bool MoveNext()
        {
            this.Index++;
            return (this.Index < this.Count);
        }

        public void Reset()
        {
            this.Index = -1;
        }
    }
}
