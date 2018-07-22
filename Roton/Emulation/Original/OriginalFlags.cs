﻿using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original
{
    [ContextEngine(ContextEngine.Original)]
    public sealed class OriginalFlags : Flags
    {
        public OriginalFlags(IMemory memory)
            : base(memory, 0x4837 + 21)
        {
        }

        public override int Count => 10;
    }
}