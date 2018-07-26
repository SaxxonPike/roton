using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Core.Impl
{
    public abstract class Scroll : IScroll
    {
        private readonly IEngine _engine;
        private readonly ITerminal _terminal;

        protected Scroll(IEngine engine, ITerminal terminal)
        {
            _engine = engine;
            _terminal = terminal;
        }

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

        protected abstract int Width { get; }

        protected abstract int Height { get; }

        protected abstract int Left { get; }

        protected abstract int Top { get; }

        protected abstract IReadOnlyList<AnsiChar> GetScreenBuffer();

        private void RenderLine(IReadOnlyList<int> chars, int y)
        {
            _terminal.Plot(Left, y, new AnsiChar(chars[0], 0x0F));
            _terminal.Plot(Left + 1, y, new AnsiChar(chars[1], 0x0F));
            for (var x = Left + 2; x < Left + Width - 2; x++)
                _terminal.Plot(x, y, new AnsiChar(chars[2], 0x0F));
            _terminal.Plot(Left + Width - 2, y, new AnsiChar(chars[3], 0x0F));
            _terminal.Plot(Left + Width - 1, y, new AnsiChar(chars[4], 0x0F));
        }

        protected abstract void RenderBuffer(IReadOnlyList<AnsiChar> buffer, int y);

        private void Open()
        {
            for (var y = Height / 2; y >= 0; y--)
            {
                var topY = Top + y;
                var bottomY = Top + Height - y - 1;

                RenderLine(ScrollCharsTop, topY);
                RenderLine(ScrollCharsBottom, bottomY);

                for (var y2 = topY + 1; y2 < bottomY - 1; y2++)
                    RenderLine(ScrollCharsMid, y2);

                _engine.WaitForTick();
            }

            RenderLine(ScrollCharsMid, Top + Height - 2);
            RenderLine(ScrollCharsSplit, Top + 2);
        }

        private void Close(IReadOnlyList<AnsiChar> buffer)
        {
            for (var y = 0; y < Height / 2; y++)
            {
                var topY = Top + y;
                var bottomY = Top + Height - y - 1;

                RenderLine(ScrollCharsTop, topY + 1);
                RenderLine(ScrollCharsBottom, bottomY - 1);
                RenderBuffer(buffer, topY);
                RenderBuffer(buffer, bottomY);

                _engine.WaitForTick();
            }

            RenderBuffer(buffer, Top + Height / 2);
        }

        private void RenderName(string name, IList<string> message, int offset)
        {
            var line = message[offset];
            var title = name;
            var pips = false;

            if (line.Length > 0 && (line[0] == ':' || line[0] == '!'))
            {
                title = "Press ENTER to select this";
                pips = true;
            }

            var x = Left + Width / 2 - title.Length / 2;
            _terminal.Write(x, Top + 1, title, 0x1E);

            // Avoid putting these directly in the string for unicode conversion reasons
            if (pips)
            {
                _terminal.Plot(x - 1, Top + 1, new AnsiChar(0xAE, 0x1E));
                _terminal.Plot(x + title.Length, Top + 1, new AnsiChar(0xAF, 0x1E));
            }
        }

        private void RenderBlank(int y)
        {
            var x = Left + 2;
            var right = Left + Width - 3;
            var blank = new AnsiChar(0x20, 0x1E);

            for (var x2 = x; x2 <= right; x2++)
                _terminal.Plot(x2, y, blank);
        }

        private void RenderPips(int y)
        {
            _terminal.Plot(Left + 2, y, new AnsiChar(0xAF, 0x1C));
            _terminal.Plot(Left + Width - 3, y, new AnsiChar(0xAE, 0x1C));
        }

        private void RenderText(string text, int y)
        {
            var x = Left + 4;
            if (text.Length < 1)
                return;

            if (text[0] == '$')
            {
                var actualText = text.Substring(1);
                _terminal.Write(Left + Width / 2 - actualText.Length / 2, y, actualText, 0x1F);
            }
            else if (text[0] == ':')
            {
                // do nothing
            }
            else if (text[0] == '!')
            {
                var actualText = text.Substring(text.IndexOf(';') + 1);
                _terminal.Plot(Left + 4, y, new AnsiChar(0x10, 0x1D));
                _terminal.Write(Left + 6, y, actualText, 0x1F);
            }
            else
            {
                _terminal.Write(x, y, text, 0x1E);
            }
        }

        private void RenderContent(string title, IList<string> message, int offset)
        {
            var center = (Height - 4) / 2;
            var line = offset - center;
            var bottom = Height + Top - 2;
            var top = Top + 3;
            var lineCount = message.Count;
            var y = top;

            RenderBlank(Top + 1);

            while (y <= bottom)
            {
                RenderBlank(y);
                if (line >= 0 && line < lineCount)
                    RenderText(message[line], y);
                else if (line == -1 || line == lineCount)
                    RenderDots(y);

                y++;
                line++;
            }

            RenderName(title, message, offset);
            RenderPips(top + center);
        }

        private void RenderDots(int y)
        {
            var x = Left + 7;
            var right = Left + Width - 3;
            var dot = new AnsiChar(0x07, 0x1E);

            for (var x2 = x; x2 <= right; x2 += 5)
                _terminal.Plot(x2, y, dot);
        }

        private bool MainLoop(string title, IList<string> message, ScrollResult result)
        {
            var update = true;

            while (_engine.ThreadActive)
            {
                if (update)
                {
                    RenderContent(title, message, result.Index);
                    update = false;
                }

                _engine.ReadInput();

                switch (_engine.State.KeyPressed)
                {
                    case EngineKeyCode.Escape:
                        return false;
                    case EngineKeyCode.Enter:
                        return true;
                    case EngineKeyCode.PageUp:
                        result.Index -= Height - 5;
                        update = true;
                        break;
                    case EngineKeyCode.PageDown:
                        result.Index += Height - 5;
                        update = true;
                        break;
                    case EngineKeyCode.Up:
                        result.Index--;
                        update = true;
                        break;
                    case EngineKeyCode.Down:
                        result.Index++;
                        update = true;
                        break;
                }

                if (update)
                {
                    if (result.Index >= message.Count)
                        result.Index = message.Count - 1;
                    if (result.Index < 0)
                        result.Index = 0;
                }

                _engine.WaitForTick();
            }

            return false;
        }

        public IScrollResult Show(string title, IEnumerable<string> message)
        {
            var msg = message.ToList();
            var buffer = GetScreenBuffer();

            var result = new ScrollResult
            {
                Index = 0,
                Label = null,
                Cancelled = false
            };

            Open();

            while (true)
            {
                var selected = MainLoop(title, msg, result);
                if (!selected)
                {
                    result.Cancelled = true;
                    break;
                }

                var innerJump = SelectLine(msg, result);
                if (!innerJump)
                    break;
            }

            Close(buffer);

            return result;
        }

        public int TextWidth => Width - 4;

        public int TextHeight => Height - 4;

        private static bool SelectLine(IReadOnlyList<string> message, ScrollResult result)
        {
            if (result.Index < 0)
                return false;

            var line = message[result.Index];

            if (!line.StartsWith("!") || !line.Contains(";"))
                return false;

            var label = line
                .Substring(1, line.IndexOf(';') - 1)
                .ToUpperInvariant();
            result.Label = label;
            label = $":{label};";

            for (var i = 0; i < message.Count; i++)
            {
                line = message[i];
                if (!line.ToUpperInvariant().StartsWith(label))
                    continue;

                result.Index = i;
                return true;
            }

            return false;
        }
    }
}