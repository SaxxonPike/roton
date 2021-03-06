﻿using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Draws.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public sealed class DefaultDraw : IDraw
    {
        public AnsiChar Draw(IXyPair location)
        {
            return new AnsiChar(0x3F, 0x40);
        }
    }
}