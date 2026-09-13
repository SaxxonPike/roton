using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Composers.Video.Scenes;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Terminal(IEnumerable<ISceneComposer> composers) : ITerminal
{
    private readonly List<ISceneComposer> _composers = [.. composers];

    public void Clear()
    {
        foreach (var composer in _composers)
            composer.Clear();
    }

    public void Plot(int x, int y, AnsiChar ac)
    {
        foreach (var composer in _composers)
            composer.Plot(x, y, ac);
    }

    public AnsiChar Read(int x, int y) =>
        _composers.FirstOrDefault()?.Read(x, y) ?? default;

    public void SetSize(int width, int height, bool wide)
    {
        foreach (var composer in _composers)
            composer.SetSize(width, height, wide);
    }

    public void Write(int x, int y, ReadOnlySpan<char> value, int color)
    {
        foreach (var composer in _composers)
            composer.Write(x, y, value, color);
    }

    public void SetFont(byte[] data)
    {
        foreach (var composer in _composers)
            composer.SetFont(data);
    }

    public void SetPalette(byte[] data)
    {
        foreach (var composer in _composers)
            composer.SetPalette(data);
    }
}