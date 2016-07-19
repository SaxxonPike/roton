using System;
using System.Collections.Generic;

namespace Roton.Emulation.Mapping
{
    internal sealed class CodeHeap
    {
        public CodeHeap()
        {
            Entries = new Dictionary<int, char[]>();
            SetNextEntry();
        }

        public char[] this[int index]
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

        public int Allocate(char[] data)
        {
            var dataCopy = new char[data.Length];
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

        private IDictionary<int, char[]> Entries { get; }

        public void FreeAll()
        {
            Entries.Clear();
            NextEntry = 1;
        }

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