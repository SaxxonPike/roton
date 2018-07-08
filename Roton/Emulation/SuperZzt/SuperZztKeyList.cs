using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.SuperZzt
{
    public sealed class SuperZztKeyList : KeyList
    {
        public SuperZztKeyList(IMemory memory) : base(memory, 0x7850)
        {
        }
    }
}