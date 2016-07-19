using System;
using System.Collections.Generic;

namespace Roton.Emulation
{
    internal sealed class Heap
    {
        public Heap()
        {
            Entries = new Dictionary<int, byte[]>();
            SetNextEntry();
        }

        public byte[] this[int index]
        {
            get
            {
                if (Entries.ContainsKey(index))
                {
                    return Entries[index];
                }
                return null;
            }
            set
            {
                if (Entries.ContainsKey(index))
                {
                    // only allow in-place replacement
                    var entry = Entries[index];
                    Array.Resize(ref entry, value.Length);
                    Array.Copy(value, entry, value.Length);
                    Entries[index] = entry;
                }
            }
        }

        public int Allocate(byte[] data)
        {
            var dataCopy = new byte[data.Length];
            Array.Copy(data, dataCopy, dataCopy.Length);
            var index = NextEntry;
            Entries[index] = dataCopy;
            SetNextEntry();
            return index;
        }

        public bool Contains(int index)
        {
            return Entries.ContainsKey(index);
        }

        private Dictionary<int, byte[]> Entries { get; }

        public bool Free(int index)
        {
            if (Entries.ContainsKey(index))
            {
                Entries.Remove(index);
                return true;
            }
            return false;
        }

        private int NextEntry { get; set; }

        private void SetNextEntry()
        {
            while (NextEntry == 0 || Contains(NextEntry))
            {
                NextEntry++;
            }
        }
    }
}