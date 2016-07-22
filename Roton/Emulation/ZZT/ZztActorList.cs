using Roton.Core;
using Roton.Emulation.Mapping;
using Roton.Extensions;

namespace Roton.Emulation.ZZT
{
    internal sealed class ZztActorList : ActorList
    {
        public ZztActorList(IMemory memory)
            : base(memory, 152)
        {
        }

        public override int Count => Memory.Read16(0x31CD) + 1;

        protected override IActor GetActor(int index)
        {
            return new Actor(Memory, 0x31CF + 0x0021*index);
        }
    }
}