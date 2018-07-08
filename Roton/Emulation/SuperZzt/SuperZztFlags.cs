using Roton.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.SuperZZT
{
    public sealed class SuperZztFlags : Flags
    {
        public SuperZztFlags(IMemory memory)
            : base(memory, 0x7863 + 21)
        {
        }

        public override int Count => 16;
    }
}