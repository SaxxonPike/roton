using System.Collections.Generic;
using Roton.Internal;

namespace Roton.Emulation
{
    internal abstract class MemoryWorldBase : World
    {
        public MemoryWorldBase(Memory memory, int worldType)
        {
            Memory = memory;
        }

        public abstract MemoryFlagArrayBase FlagMemory { get; }

        public override IList<string> Flags
        {
            get { return FlagMemory; }
            protected set { }
        }

        public abstract MemoryKeyArray KeyMemory { get; }

        public override IList<bool> Keys
        {
            get { return KeyMemory; }
            protected set { }
        }

        public Memory Memory { get; private set; }
    }
}