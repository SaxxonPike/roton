using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [Context(Context.Original)]
    public sealed class OriginalFlags : Flags
    {
        public OriginalFlags(IMemory memory)
            : base(memory, 0x4837 + 21)
        {
        }

        public override int Count => 10;
    }
}