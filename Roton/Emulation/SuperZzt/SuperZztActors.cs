using Roton.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    public sealed class SuperZztActors : Actors
    {
        public SuperZztActors(IMemory memory)
            : base(memory, 129)
        {
        }

        public override int Count => Memory.Read16(0x6AB3) + 1;

        protected override IActor GetActor(int index)
        {
            return new Actor(Memory, 0x6AB5 + 0x0019*index);
        }
    }
}