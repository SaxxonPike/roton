using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super
{
    [Context(Context.Super)]
    public sealed class SuperKeyList : KeyList
    {
        public SuperKeyList(Lazy<IMemory> memory) : base(memory, 0x7850)
        {
        }
    }
}