using System;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;

namespace Roton.Composers.Video.Scenes.Impl;

/// <summary>
/// Implements a null scene composer.
/// </summary>
/// <remarks>
/// Characters are still kept in a buffer, but no bitmap will be generated.
/// </remarks>
public sealed class NullSceneComposer : ISceneComposer
{
    private AnsiChar[] _chars = [];

    public event EventHandler<ResizedEventArgs>? Resized;

    public void Clear() =>
        _chars.AsSpan().Clear();

    private int GetIndex(int x, int y) =>
        y * Columns + x;

    public void Plot(int x, int y, AnsiChar ac)
    {
        var idx = GetIndex(x, y);
        if (idx < 0 || idx >= _chars.Length)
            return;
        _chars[idx] = ac;
    }

    public AnsiChar Read(int x, int y)
    {
        var idx = GetIndex(x, y);
        if (idx < 0 || idx >= _chars.Length)
            return default;
        return _chars[idx];
    }

    public void SetSize(int width, int height, bool wide)
    {
        if (Rows != height || Columns != width)
            _chars = new AnsiChar[width * height];

        Rows = height;
        Columns = width;
        Wide = wide;
        Resized?.Invoke(this, new ResizedEventArgs(width, height, wide));
    }

    public void Write(int x, int y, ReadOnlySpan<char> value, int color)
    {
        var i = 0;
        foreach (var c in value)
            Plot(x + i++, y, new AnsiChar(Cp437.CharToByte(c), color));
    }

    public void SetFont(byte[] data)
    {
    }

    public void SetPalette(byte[] data)
    {
    }

    public int Rows { get; private set; }

    public int Columns { get; private set; }

    public Bitmap? GetBitmap(bool onlyIfUpdated) =>
        null;

    public bool UseFullBrightBackgrounds { get; set; }

    public bool Wide { get; private set; }
}