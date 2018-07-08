using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Zzt
{
    public sealed class ZztKeyList : KeyList
    {
        public ZztKeyList(IMemory memory) : base(memory, 0x4822)
        {
        }
    }
}