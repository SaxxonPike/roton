using System;

namespace Roton.Emulation.Core;

public static class TerminalExtensions
{
    extension(ITerminal terminal)
    {
        public void Write(int x,
            int y,
            ReadOnlySpan<char> text0,
            ReadOnlySpan<char> text1,
            int color)
        {
            var ix = x;
            terminal.Write(ix, y, text0, color);
            ix += text0.Length;
            terminal.Write(ix, y, text1, color);
        }

        public void Write(int x,
            int y,
            ReadOnlySpan<char> text0,
            ReadOnlySpan<char> text1,
            ReadOnlySpan<char> text2,
            int color)
        {
            var ix = x;
            terminal.Write(ix, y, text0, color);
            ix += text0.Length;
            terminal.Write(ix, y, text1, color);
            ix += text1.Length;
            terminal.Write(ix, y, text2, color);
        }
    }
}