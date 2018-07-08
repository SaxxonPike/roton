using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.ZZT
{
    public sealed class ZztFlags : Flags
    {
        public ZztFlags(IMemory memory)
            : base(memory, 0x4837 + 21)
        {
        }

        public override int Count => 10;
    }
}