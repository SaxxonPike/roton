using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original
{
    [ContextEngine(ContextEngine.Original)]
    public sealed class OriginalKeyList : KeyList
    {
        public OriginalKeyList(IMemory memory) : base(memory, 0x4822)
        {
        }
    }
}