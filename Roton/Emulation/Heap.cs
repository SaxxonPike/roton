using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    sealed internal class Heap
    {
        public Heap()
        {
            Entries = new Dictionary<int, byte[]>();
            NextEntry = 0;
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
            byte[] dataCopy = new byte[data.Length];
            Array.Copy(data, dataCopy, dataCopy.Length);
            Entries[NextEntry++] = dataCopy;
            return NextEntry - 1;
        }

        public bool Contains(int index)
        {
            return Entries.ContainsKey(index);
        }

        private Dictionary<int, byte[]> Entries
        {
            get;
            set;
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

        private int NextEntry
        {
            get;
            set;
        }
    }
}
