using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.ZZT
{
    sealed internal partial class MemoryElementCollection : MemoryElementCollectionBase
    {
        public MemoryElementCollection(Memory memory)
            : base(memory)
        {
            memory.Write(0x4AD4, Properties.Resources.zztelement);
        }

        protected override MemoryElementBase GetElement(int index)
        {
            return new MemoryElement(Memory, index);
        }

        public override int Count
        {
            get { return 54; }
        }
    }
}
