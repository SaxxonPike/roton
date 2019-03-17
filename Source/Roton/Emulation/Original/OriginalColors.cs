using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [Context(Context.Original)]
    public sealed class OriginalColors : Colors
    {
        public OriginalColors(IMemory memory)
            : base(memory, 0xFFF9)
        {
        }

        public override int Count => 7;
    }
}