using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Zzt
{
    public sealed class ZztColors : Colors
    {
        public ZztColors(IMemory memory)
            : base(memory, 0xFFF9)
        {
        }

        public override int Count => 7;
    }
}