using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.Zzt
{
    public sealed class ZztKeyList : KeyList
    {
        public ZztKeyList(IMemory memory) : base(memory, 0x4822)
        {
        }
    }
}