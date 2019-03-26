using System;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [Context(Context.Original)]
    public sealed class OriginalKeyList : KeyList
    {
        public OriginalKeyList(Lazy<IMemory> memory) : base(memory, 0x4822)
        {
        }
    }
}