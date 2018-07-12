using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super
{
    [ContextEngine(ContextEngine.Super)]
    public sealed class SuperKeyList : KeyList
    {
        public SuperKeyList(IMemory memory) : base(memory, 0x7850)
        {
        }
    }
}