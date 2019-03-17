using System;
using System.Collections.Generic;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [Context(Context.Original)]
    public sealed class LongTextEntryHud : ILongTextEntryHud
    {
        private readonly Lazy<ITerminal> _terminal;
        private readonly Lazy<ITextEntryHud> _textEntryHud;

        public LongTextEntryHud(Lazy<ITerminal> terminal, Lazy<ITextEntryHud> textEntryHud)
        {
            _terminal = terminal;
            _textEntryHud = textEntryHud;
        }

        private ITerminal Terminal => _terminal.Value;
        private ITextEntryHud TextEntryHud => _textEntryHud.Value;
        
        private static readonly int[] ScrollCharsTop =
        {
            0xC6, 0xD1, 0xCD, 0xD1, 0xB5
        };

        private static readonly int[] ScrollCharsMid =
        {
            0x20, 0xB3, 0x20, 0xB3, 0x20
        };

        private static readonly int[] ScrollCharsSplit =
        {
            0x20, 0xC6, 0xCD, 0xB5, 0x20
        };

        private static readonly int[] ScrollCharsBottom =
        {
            0xC6, 0xCF, 0xCD, 0xCF, 0xB5
        };
        
        private IReadOnlyList<AnsiChar> LoadBuffer(int left, int top, int width, int height)
        {
            var buffer = new AnsiChar[width * height];
            var i = 0;
            
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                buffer[i++] = Terminal.Read(x + left, y + top);

            return buffer;
        }
        
        private void RestoreBuffer(IReadOnlyList<AnsiChar> buffer, int left, int top, int width, int height)
        {
            var i = 0;
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                Terminal.Plot(x + left, y + top, buffer[i++]);
        }        

        public string Show(string title, int x, int y, int maxLength, int textColor, int pipColor)
        {
            var width = maxLength + 15;
            var titleX = x + 2 + (width - title.Length) / 2;
            const int height = 6;

            void RenderLine(int lineY, IReadOnlyList<int> chars)
            {
                Terminal.Plot(x, lineY, new AnsiChar(chars[0], pipColor));
                Terminal.Plot(x + 1, lineY, new AnsiChar(chars[1], pipColor));
                for (var lineX = x + 2; lineX < x + width - 2; lineX++)
                    Terminal.Plot(lineX, lineY, new AnsiChar(chars[2], pipColor));
                Terminal.Plot(x + width - 2, lineY, new AnsiChar(chars[3], pipColor));
                Terminal.Plot(x + width - 1, lineY, new AnsiChar(chars[4], pipColor));
            }

            var buffer = LoadBuffer(x, y, width, height);
            
            RenderLine(y, ScrollCharsTop);
            RenderLine(y + 1, ScrollCharsMid);
            RenderLine(y + 2, ScrollCharsSplit);
            RenderLine(y + 3, ScrollCharsMid);
            RenderLine(y + 4, ScrollCharsMid);
            RenderLine(y + 5, ScrollCharsBottom);
            Terminal.Write(titleX, y + 1, title, pipColor);

            var result = TextEntryHud.Show(x + 7, y + 3, maxLength, textColor, pipColor);
            
            RestoreBuffer(buffer, x, y, width, height);
            return result;
        }
    }
}