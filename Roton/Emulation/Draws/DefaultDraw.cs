﻿using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Draws
{
    [ContextEngine(ContextEngine.Zzt)]
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class DefaultDraw : IDraw
    {
        public AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(0x3F, 0x40);
        }
    }
}