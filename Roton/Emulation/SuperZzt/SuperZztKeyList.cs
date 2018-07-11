using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.SuperZzt
{
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class SuperZztKeyList : KeyList
    {
        public SuperZztKeyList(IMemory memory) : base(memory, 0x7850)
        {
        }
    }
}