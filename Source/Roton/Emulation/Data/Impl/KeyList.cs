using System;
using System.Diagnostics;

namespace Roton.Emulation.Data.Impl
{
    public abstract class KeyList : FixedList<bool>, IKeyList
    {
        private readonly Lazy<IMemory> _memory;
        private readonly int _offset;

        protected KeyList(Lazy<IMemory> memory, int offset)
        {
            _memory = memory;
            _offset = offset;
        }

        private IMemory Memory
        {
            [DebuggerStepThrough] get => _memory.Value;
        }

        public override int Count => 7;

        public override void Clear()
        {
            for (var i = 0; i < Count; i++)
                this[i] = false;
        }

        protected override bool GetItem(int index) 
            => Memory.ReadBool(_offset + index);

        protected override void SetItem(int index, bool value) 
            => Memory.WriteBool(_offset + index, value);
    }
}