using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "DARK")]
public sealed class DarkCheat(Lazy<IEngine> engine) : ICheat
{
    private IEngine Engine => engine.Value;

    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        Engine.Board.IsDark = !clear;
        Engine.Hud.RedrawBoard();
    }
}