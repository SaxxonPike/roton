﻿using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Core
{
    public interface ITerminal
    {
        void Clear();
        void Plot(int x, int y, AnsiChar ac);
        AnsiChar Read(int x, int y);
        void SetSize(int width, int height, bool wide);
        void Write(int x, int y, string value, int color);
        void SetFont(byte[] data);
        void SetPalette(byte[] data);
    }
}