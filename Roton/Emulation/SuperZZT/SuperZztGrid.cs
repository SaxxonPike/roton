using Roton.Core;
using Roton.Emulation.Mapping;

namespace Roton.Emulation.SuperZZT
{
    public sealed class SuperZztGrid : Grid
    {
        public SuperZztGrid(IMemory memory, IElements elements)
            : base(memory, elements, 0x2BEB, 96, 80)
        {
        }
    }
}