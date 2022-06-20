using System;
using System.Collections.Generic;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalScroll : Scroll
{
    public OriginalScroll(Lazy<IEngine> engine, Lazy<ITerminal> terminal) : base(engine, terminal)
    {
    }

    protected override int Width => 49;
    protected override int Height => 19;
    protected override int Left => 5;
    protected override int Top => 3;
        
    protected override IReadOnlyList<AnsiChar> GetScreenBuffer()
    {
        return Array.Empty<AnsiChar>();
    }

    protected override void RenderBuffer(IReadOnlyList<AnsiChar> buffer, int y)
    {
        for (var x = Left; x < Left + Width; x++)
            Engine.UpdateBoard(new Location(x + 1, y + 1));
    }
}