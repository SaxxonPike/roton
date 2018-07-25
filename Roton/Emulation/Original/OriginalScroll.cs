using System.Collections.Generic;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [ContextEngine(ContextEngine.Original)]
    public sealed class OriginalScroll : Scroll
    {
        private readonly IEngine _engine;

        public OriginalScroll(IEngine engine, ITerminal terminal) : base(engine, terminal)
        {
            _engine = engine;
        }

        protected override int Width => 49;
        protected override int Height => 19;
        protected override int Left => 5;
        protected override int Top => 3;
        
        protected override IReadOnlyList<AnsiChar> GetScreenBuffer()
        {
            return new AnsiChar[0];
        }

        protected override void RenderBuffer(IReadOnlyList<AnsiChar> buffer, int y)
        {
            for (var x = Left; x < Left + Width; x++)
                _engine.UpdateBoard(new Location(x + 1, y + 1));
        }
    }
}