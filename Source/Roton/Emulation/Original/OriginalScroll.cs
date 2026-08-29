using System.Collections.Generic;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalScroll(
    IEngineAccessor engine,
    ITerminal terminal,
    IState state,
    IFileSystem fileSystem,
    IBoardUpdater boardUpdater,
    IScrollContent scrollContent)
    : Scroll(engine, terminal, state, fileSystem, scrollContent)
{
    protected override int Width => 49;
    protected override int Height => 19;
    protected override int Left => 5;
    protected override int Top => 3;

    protected override IReadOnlyList<AnsiChar> GetScreenBuffer()
    {
        return [];
    }

    protected override void RenderBuffer(IReadOnlyList<AnsiChar> buffer, int y)
    {
        for (var x = Left; x < Left + Width; x++)
            boardUpdater.UpdateBoard(new Location(x + 1, y + 1));
    }
}