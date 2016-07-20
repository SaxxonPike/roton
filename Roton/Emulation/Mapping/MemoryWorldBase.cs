using System.Collections.Generic;
using Roton.Core;
using Roton.Emulation.Models;

namespace Roton.Emulation.Mapping
{
    internal abstract class MemoryWorldBase : World
    {
        public MemoryWorldBase(IMemory memory, int worldType)
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

        public IMemory Memory { get; private set; }
    }
}