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
                var bottomY = Top + Height - y;

                RenderLine(ScrollCharsTop, topY);
                RenderLine(ScrollCharsBottom, bottomY);

                for (var y2 = topY + 1; y2 < bottomY - 1; y2++)
                    RenderLine(ScrollCharsMid, y2);

                _engine.WaitForTick();
            }

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

        private void RenderBlank(int y)
        {
            var x = Left + 3;
            var right = Left + Width - 3;
            var blank = new AnsiChar(0x20, 0x1E);
            
            for (var x2 = x; x2 <= right; x2++)
                _terminal.Plot(x2, y, blank);                                
        }

        private void RenderContent(IList<string> message, int offset)
        {
            var line = offset - (Height - 4) / 2;
            var bottom = Height + Top - 2;
            var top = Top + 3;
            var lineCount = message.Count;
            var x = Left + 3;
            var right = Left + Width - 3;
            var y = top;

            while (y <= bottom)
            {
                RenderBlank(y);
                if (line >= 0 && line < lineCount)
                    _terminal.Write(x, y, message[line], 0x1E);
                
                y++;
                line++;
            }
        }

        private int MainLoop(IList<string> message)
        {
            var offset = 0;
            var update = true;

            while (_engine.ThreadActive)
            {
                if (update)
                {
                    RenderContent(message, offset);
                    update = false;
                }

                _engine.ReadInput();

                switch (_engine.State.KeyPressed)
                {
                    case EngineKeyCode.Escape:
                        return -1;
                    case EngineKeyCode.Enter:
                        return -1;
                    case EngineKeyCode.Up:
                        offset--;
                        update = true;
                        break;
                    case EngineKeyCode.Down:
                        offset++;
                        update = true;
                        break;
                }

                _engine.WaitForTick();
            }

            return -1;
        }

        public IScrollResult Show(IEnumerable<string> message)
        {
            var msg = message.ToList();
            var buffer = GetScreenBuffer();

            Open();
            var line = MainLoop(msg);
            Close(buffer);

            return new ScrollResult
            {
                SelectedLine = line
            };
        }
    }
}