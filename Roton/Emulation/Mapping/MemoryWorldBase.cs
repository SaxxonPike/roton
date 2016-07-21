using Roton.Core;
using Roton.Emulation.Models;

namespace Roton.Emulation.Mapping
{
    internal abstract class MemoryWorldBase : World
    {
        protected MemoryWorldBase(IMemory memory)
        {
            Memory = memory;
        }

        public abstract override IFlagList Flags { get; }

        public abstract override IKeyList Keys { get; }

        protected IMemory Memory { get; private set; }
    }
}