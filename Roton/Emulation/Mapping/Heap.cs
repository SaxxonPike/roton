using System;
using System.Collections.Generic;
using Roton.Core;

namespace Roton.Emulation.Mapping
{
    internal sealed class Heap : IHeap
    {
        public Heap()
        {
            Entries = new Dictionary<int, byte[]>();
            SetNextEntry();
        }

        private IDictionary<int, byte[]> Entries { get; }

        private int NextEntry { get; set; }

        public int Allocate(byte[] data)
        {
            var dataCopy = new byte[data.Length];
            Array.Copy(data, dataCopy, dataCopy.Length);
            var index = NextEntry;
            Entries[index] = dataCopy;
            SetNextEntry();
            return index;
        }

        public void FreeAll()
        {
            Entries.Clear();
            NextEntry = 1;
        }

        public byte[] this[int index] => Entries.ContainsKey(index)
            ? Entries[index]
            : null;

        private bool Contains(int index) => Entries.ContainsKey(index);

        private void SetNextEntry()
        {
            while (NextEntry == 0 || Contains(NextEntry))
            {
                NextEntry++;
            }
        }
    }
}