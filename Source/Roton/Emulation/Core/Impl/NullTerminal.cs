using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class NullTerminal : ITerminal
{
    public void Clear()
    {
    }

    public void Plot(int x, int y, AnsiChar ac)
    {
    }

    public AnsiChar Read(int x, int y) => default;

    public void SetSize(int width, int height, bool wide)
    {
    }

    public void Write(int x, int y, ReadOnlySpan<char> value, int color)
    {
    }

    public void SetFont(byte[] data)
    {
    }

    public void SetPalette(byte[] data)
    {
    }
}