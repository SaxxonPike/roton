using System;
using System.Collections.Generic;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super
{
    [Context(Context.Super)]
    public sealed class SuperScroll : Scroll
    {
        public SuperScroll(Lazy<IEngine> engine, Lazy<ITerminal> terminal) : base(engine, terminal)
        {
        }

        protected override int Width => 37;
        protected override int Height => 23;
        protected override int Left => 1;
        protected override int Top => 2;
        
        protected override IReadOnlyList<AnsiChar> GetScreenBuffer()
        {
            var buffer = new AnsiChar[Width * Height];
            var i = 0;
            
            for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                buffer[i++] = Terminal.Read(x + Left, y + Top);

            return buffer;
        }

        protected override void RenderBuffer(IReadOnlyList<AnsiChar> buffer, int y)
        {
            var i = Width * (y - Top);
            for (var x = Left; x < Left + Width; x++)
                Terminal.Plot(x, y, buffer[i++]);
        }
    }
}