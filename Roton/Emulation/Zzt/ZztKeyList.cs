using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Zzt
{
    [ContextEngine(ContextEngine.Zzt)]
    public sealed class ZztKeyList : KeyList
    {
        public ZztKeyList(IMemory memory) : base(memory, 0x4822)
        {
        }
    }
}