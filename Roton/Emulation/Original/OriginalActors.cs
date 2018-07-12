using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original
{
    [ContextEngine(ContextEngine.Original)]
    public sealed class OriginalActors : Actors
    {
        public OriginalActors(IMemory memory)
            : base(memory, 152)
        {
        }

        public override int Count => Memory.Read16(0x31CD) + 1;

        protected override IActor GetActor(int index)
        {
            return new Actor(Memory, 0x31CF + 0x0021 * index);
        }
    }
}