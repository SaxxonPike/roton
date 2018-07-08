using Roton.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.SuperZZT
{
    public sealed class SuperZztKeyList : KeyList
    {
        public SuperZztKeyList(IMemory memory) : base(memory, 0x7850)
        {
        }
    }
}