using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation
{
    internal abstract class MemoryWorldBase : World
    {
        public MemoryWorldBase(Memory memory, int worldType)
            : base()
        {
            this.Memory = memory;
        }

        abstract public MemoryFlagArrayBase FlagMemory { get; }

        public override IList<string> Flags
        {
            get
            {
                return FlagMemory;
            }
            protected set
            {
            }
        }

        abstract public MemoryKeyArray KeyMemory { get; }

        public override IList<bool> Keys
        {
            get
            {
                return KeyMemory;
            }
            protected set
            {
            }
        }

        public Memory Memory
        {
            get;
            private set;
        }
    }
}
