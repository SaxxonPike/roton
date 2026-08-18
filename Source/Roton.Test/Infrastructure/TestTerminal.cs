using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Infrastructure;

public class TestTerminal : ITerminal
{
    public void Clear()
    {
    }

    public void Plot(int x, int y, AnsiChar ac)
    {
    }

    public AnsiChar Read(int x, int y)
    {
        return default;
    }

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