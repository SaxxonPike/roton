﻿using System.Linq;

namespace Roton.Emulation.Data.Impl
{
    public abstract class Flags : FixedList<string>, IFlags
    {
        protected Flags(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        private IMemory Memory { get; }

        private int Offset { get; }

        public override void Add(string item)
        {
            var count = Count;
            for (var i = 0; i < count; i++)
            {
                if (!string.IsNullOrEmpty(GetItem(i)))
                    continue;

                SetItem(i, item);
                return;
            }
        }

        public override void Clear()
        {
            for (var i = 0; i < Count; i++)
            {
                this[i] = string.Empty;
            }
        }

        public override bool Contains(string item)
        {
            var count = Count;
            for (var i = 0; i < count; i++)
            {
                if (GetItem(i) == item)
                    return true;
            }
            return false;
        }

        public override bool Remove(string item)
        {
            var count = Count;
            for (var i = 0; i < count; i++)
            {
                if (GetItem(i) != item)
                    continue;

                SetItem(i, string.Empty);
                return true;
            }
            return false;
        }

        public string StoneText
        {
            get
            {
                foreach (var flag in this.Select(f => f.ToUpperInvariant()))
                {
                    if (flag.Length > 0 && flag.StartsWith("Z"))
                    {
                        return flag.Substring(1);
                    }
                }

                return string.Empty;
            }
        }

        protected override string GetItem(int index)
        {
            return Memory.ReadString(Offset + index*21);
        }

        protected override void SetItem(int index, string value)
        {
            Memory.WriteString(Offset + index*21, value);
        }
    }
}