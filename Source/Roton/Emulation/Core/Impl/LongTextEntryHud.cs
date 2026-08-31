using System;
using System.Collections.Generic;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
internal sealed class LongTextEntryHud(ITerminal terminal, ITextEntryHud textEntryHud) : ILongTextEntryHud
{
    private static readonly int[] ScrollCharsTop =
    [
        0xC6, 0xD1, 0xCD, 0xD1, 0xB5
    ];

    private static readonly int[] ScrollCharsMid =
    [
        0x20, 0xB3, 0x20, 0xB3, 0x20
    ];

    private static readonly int[] ScrollCharsSplit =
    [
        0x20, 0xC6, 0xCD, 0xB5, 0x20
    ];

    private static readonly int[] ScrollCharsBottom =
    [
        0xC6, 0xCF, 0xCD, 0xCF, 0xB5
    ];
        
    private AnsiChar[] LoadBuffer(int left, int top, int width, int height)
    {
        var buffer = new AnsiChar[width * height];
        var i = 0;
            
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            buffer[i++] = terminal.Read(x + left, y + top);

        return buffer;
    }
        
    private void RestoreBuffer(IReadOnlyList<AnsiChar> buffer, int left, int top, int width, int height)
    {
        var i = 0;
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            terminal.Plot(x + left, y + top, buffer[i++]);
    }        

    public string Show(string title, int x, int y, int maxLength, int textColor, int pipColor)
    {
        var width = maxLength + 15;
        var titleX = x + 2 + (width - title.Length) / 2;
        const int height = 6;

        var buffer = LoadBuffer(x, y, width, height);
            
        RenderLine(y, ScrollCharsTop);
        RenderLine(y + 1, ScrollCharsMid);
        RenderLine(y + 2, ScrollCharsSplit);
        RenderLine(y + 3, ScrollCharsMid);
        RenderLine(y + 4, ScrollCharsMid);
        RenderLine(y + 5, ScrollCharsBottom);
        terminal.Write(titleX, y + 1, title, pipColor);

        var result = textEntryHud.Show(x + 7, y + 3, maxLength, textColor, pipColor, ReadOnlySpan<char>.Empty);
            
        RestoreBuffer(buffer, x, y, width, height);
        return result;

        void RenderLine(int lineY, IReadOnlyList<int> chars)
        {
            terminal.Plot(x, lineY, new AnsiChar(chars[0], pipColor));
            terminal.Plot(x + 1, lineY, new AnsiChar(chars[1], pipColor));
            for (var lineX = x + 2; lineX < x + width - 2; lineX++)
                terminal.Plot(lineX, lineY, new AnsiChar(chars[2], pipColor));
            terminal.Plot(x + width - 2, lineY, new AnsiChar(chars[3], pipColor));
            terminal.Plot(x + width - 1, lineY, new AnsiChar(chars[4], pipColor));
        }
    }
}