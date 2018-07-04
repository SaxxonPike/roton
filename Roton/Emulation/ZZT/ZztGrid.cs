using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.ZZT
{
    public sealed class ZztGrid : Grid
    {
        public ZztGrid(IMemory memory, IElements elements)
            : base(memory, elements, 0x24B9, 60, 25)
        {
        }
    }
}