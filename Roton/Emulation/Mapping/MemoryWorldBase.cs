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

        public abstract IFlagList FlagMemory { get; }

        public override IFlagList Flags
        {
            get { return FlagMemory; }
            protected set { }
        }

        public abstract IKeyList KeyMemory { get; }

        public override IKeyList Keys
        {
            get { return KeyMemory; }
            protected set { }
        }

        public IMemory Memory { get; private set; }
    }
}