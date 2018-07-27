using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Data.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class Heap : IHeap
    {
        private int _nextEntry;

        public Heap()
        {
            Entries = new Dictionary<int, byte[]>();
            SetNextEntry();
        }

        private IDictionary<int, byte[]> Entries { get; }

        public int Size => Entries.Where(e => e.Value != null).Sum(e => e.Value.Length);

        public int Allocate(byte[] data)
        {
            var dataCopy = new byte[data.Length];
            Array.Copy(data, dataCopy, dataCopy.Length);
            var index = _nextEntry;
            Entries[index] = dataCopy;
            SetNextEntry();
            return index;
        }

        public void FreeAll()
        {
            Entries.Clear();
            _nextEntry = 1;
        }

        public byte[] this[int index] => Entries.ContainsKey(index)
            ? Entries[index]
            : null;

        private bool Contains(int index) => Entries.ContainsKey(index);

        private void SetNextEntry()
        {
            while (_nextEntry == 0 || Contains(_nextEntry))
            {
                _nextEntry++;
            }
        }
    }
}