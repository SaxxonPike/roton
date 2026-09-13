using System;
using System.Collections.Generic;
using Roton.Composers.Video.Scenes;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Terminal(IEnumerable<ISceneComposer> composers) : ITerminal
{
    private readonly List<ISceneComposer> _composers = [.. composers];

    /// <summary>
    /// Character buffer.
    /// </summary>
    private Memory<AnsiChar> _chars = new AnsiChar[80 * 25];

    private int _maxIndex = 80 * 25;

    private int _columns = 80;

    /// <summary>
    /// Gets the offset into the character array based on X/Y coordinates.
    /// </summary>
    private int GetBufferOffset(int x, int y) =>
        x + y * _columns;

    public void Clear()
    {
        _chars.Span.Clear();

        foreach (var composer in _composers)
            composer.Clear();
    }

    public void Plot(int x, int y, AnsiChar ac)
    {
        var idx = GetBufferOffset(x, y);

        if (idx >= 0 && idx < _maxIndex)
            _chars.Span[idx] = ac;

        foreach (var composer in _composers)
            composer.Plot(x, y, ac);
    }

    public AnsiChar Read(int x, int y)
    {
        var idx = GetBufferOffset(x, y);

        return idx >= 0 && idx < _maxIndex
            ? _chars.Span[idx]
            : default;
    }

    public void SetSize(int width, int height, bool wide)
    {
        if (_chars.Length < width * height)
            _chars = new AnsiChar[width * height];

        _maxIndex = width * height;
        _columns = width;

        foreach (var composer in _composers)
            composer.SetSize(width, height, wide);
    }

    public void Write(int x, int y, ReadOnlySpan<char> value, int color)
    {
        foreach (var c in value)
            Plot(x++, y, new AnsiChar(Cp437.CharToByte(c), color));
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