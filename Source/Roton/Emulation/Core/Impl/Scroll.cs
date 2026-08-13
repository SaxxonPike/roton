using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Core.Impl;

public abstract class Scroll : IScroll
{
    private readonly Lazy<IEngine> _engine;
    private readonly Lazy<ITerminal> _terminal;

    protected Scroll(Lazy<IEngine> engine, Lazy<ITerminal> terminal)
    {
        _engine = engine;
        _terminal = terminal;
    }

    protected IEngine Engine
    {
        [DebuggerStepThrough] get => _engine.Value;
    }

    protected ITerminal Terminal
    {
        [DebuggerStepThrough] get => _terminal.Value;
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
        Terminal.Plot(Left, y, new AnsiChar(chars[0], 0x0F));
        Terminal.Plot(Left + 1, y, new AnsiChar(chars[1], 0x0F));
        for (var x = Left + 2; x < Left + Width - 2; x++)
            Terminal.Plot(x, y, new AnsiChar(chars[2], 0x0F));
        Terminal.Plot(Left + Width - 2, y, new AnsiChar(chars[3], 0x0F));
        Terminal.Plot(Left + Width - 1, y, new AnsiChar(chars[4], 0x0F));
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

            Engine.WaitForTick();
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

            Engine.WaitForTick();
        }

        RenderBuffer(buffer, Top + Height / 2);
    }

    private void RenderName(string title, IList<string> message, int offset)
    {
        var line = message[offset];
        var pips = false;

        if (line.Length > 0 && (line[0] == ':' || line[0] == '!'))
        {
            title = "Press ENTER to select this";
            pips = true;
        }

        var x = Left + Width / 2 - title.Length / 2;
        Terminal.Write(x, Top + 1, title, 0x1E);

        // Avoid putting these directly in the string for unicode conversion reasons
        if (pips)
        {
            Terminal.Plot(x - 1, Top + 1, new AnsiChar(0xAE, 0x1E));
            Terminal.Plot(x + title.Length, Top + 1, new AnsiChar(0xAF, 0x1E));
        }
    }

    private void RenderBlank(int y)
    {
        var x = Left + 2;
        var right = Left + Width - 3;
        var blank = new AnsiChar(0x20, 0x1E);

        for (var x2 = x; x2 <= right; x2++)
            Terminal.Plot(x2, y, blank);
    }

    private void RenderPips(int y)
    {
        Terminal.Plot(Left + 2, y, new AnsiChar(0xAF, 0x1C));
        Terminal.Plot(Left + Width - 3, y, new AnsiChar(0xAE, 0x1C));
    }

    private void RenderText(string text, int y)
    {
        var x = Left + 4;
        if (text.Length < 1)
            return;

        if (text[0] == '$')
        {
            var actualText = text.Substring(1);
            Terminal.Write(Left + Width / 2 - actualText.Length / 2, y, actualText, 0x1F);
        }
        else if (text[0] == ':')
        {
            if (text.Contains(';'))
            {
                var actualText = text.Substring(text.IndexOf(';') + 1);
                Terminal.Write(x, y, actualText, 0x1F);                    
            }
        }
        else if (text[0] == '!')
        {
            var actualText = text.Substring(text.IndexOf(';') + 1);
            Terminal.Plot(Left + 4, y, new AnsiChar(0x10, 0x1D));
            Terminal.Write(Left + 6, y, actualText, 0x1F);
        }
        else
        {
            Terminal.Write(x, y, text, 0x1E);
        }
    }

    private void RenderContent(IScrollState state)
    {
        var offset = state.Index;
        var message = state.Lines;
        var title = state.Title;
            
        var center = (Height - 4) / 2;
        var line = offset - center;
        var bottom = Height + Top - 2;
        var top = Top + 3;
        var lineCount = message.Count;
        var y = top;

        RenderBlank(Top + 1);

        for (var y2 = y; y2 <= bottom; y2++)
            RenderBlank(y2);

        RenderPips(top + center);

        while (y <= bottom)
        {

            if (state.IsHelp)
            {
                if (line == -5)
                {
                    Terminal.Write(Left + 5, y, "Use            to view text,", 0x1A);
                    Terminal.Write(Left + 9, y, "\u2191 \u2193, Enter", 0x1F);
                }
                else if (line == -4)
                {
                    Terminal.Write(Left + 20, y, "to print.", 0x1A);
                    Terminal.Write(Left + 14, y, "Alt-P", 0x1F);
                }
            }
                
            if (line >= 0 && line < lineCount)
                RenderText(message[line], y);
            else if (line == -1 || line == lineCount)
                RenderDots(y);

            y++;
            line++;
        }

        RenderName(title, message, offset);
    }

    private void RenderDots(int y)
    {
        var x = Left + 6;
        var right = Left + Width - 3;
        var dot = new AnsiChar(0x07, 0x1E);

        for (var x2 = x; x2 <= right; x2 += 5)
            Terminal.Plot(x2, y, dot);
    }

    private bool MainLoop(IScrollState state)
    {
        var update = false;

        while (Engine.ThreadActive)
        {
            if (update)
            {
                RenderContent(state);
                update = false;
            }

            Engine.ReadInput();

            switch (Engine.State.KeyPressed)
            {
                case EngineKeyCode.Escape:
                    return false;
                case EngineKeyCode.Enter:
                    return true;
                case EngineKeyCode.PageUp:
                    state.Index -= Height - 5;
                    update = true;
                    break;
                case EngineKeyCode.PageDown:
                    state.Index += Height - 5;
                    update = true;
                    break;
                case EngineKeyCode.Up:
                    state.Index--;
                    update = true;
                    break;
                case EngineKeyCode.Down:
                    state.Index++;
                    update = true;
                    break;
            }

            if (update)
            {
                if (state.Index >= state.Lines.Count)
                    state.Index = state.Lines.Count - 1;
                if (state.Index < 0)
                    state.Index = 0;
            }

            Engine.WaitForTick();
        }

        return false;
    }
        
    private bool LoadHelpFile(IScrollState state, string filename)
    {
        var text = Engine.Disk
            .GetFile($"{filename}.HLP")?
            .ToStringValue()
            .Replace("\xD\xA", "\xD")
            .Split('\xD');

        if (text == null)
            return false;

        state.Lines = text;
        state.Index = 0;
        state.IsHelp = true;
        return true;
    }

    private void ShowLoop(IScrollState state)
    {
        while (true)
        {
            RenderContent(state);
            var selected = MainLoop(state);
            if (!selected)
            {
                state.Cancelled = true;
                break;
            }

            var innerJump = SelectLine(state);
            if (!innerJump)
                break;
        }                
    }

    private IScrollState Show(IScrollState state, Action<IScrollState> mainLoop)
    {
        var buffer = GetScreenBuffer();
        Open();
        RenderContent(state);
        mainLoop(state);
        Close(buffer);
        return state;
    }
        
    public IScrollState Show(string title, string fileName)
    {
        var state = new ScrollState
        {
            Index = 0,
            Label = null,
            Cancelled = false,
            IsHelp = true,
            Title = title
        };

        if (LoadHelpFile(state, fileName))
            return Show(state, ShowLoop);

        return state;
    }
        
    public IScrollState Show(string title, IEnumerable<string> message, bool isHelp, int index) 
        => Show(title, message, isHelp, index, ShowLoop);

    public IScrollState Show(string title, IEnumerable<string> message, bool isHelp, int index, Action<IScrollState> mainLoop)
    {
        var state = new ScrollState
        {
            Index = index,
            Label = null,
            Cancelled = false,
            Lines = message.ToList(),
            IsHelp = isHelp,
            Title = title
        };

        return Show(state, mainLoop);
    }

    public int TextWidth => Width - 4;

    public int TextHeight => Height - 4;

    private bool SelectLine(IScrollState state)
    {
        if (state.Index < 0)
            return false;

        var line = state.Lines[state.Index];

        if (!line.StartsWith("!") || !line.Contains(";"))
            return false;

        var label = line
            .Substring(1, line.IndexOf(';') - 1)
            .ToUpperInvariant();

        if (line.StartsWith("!") && label.StartsWith("-") && LoadHelpFile(state, label.Substring(1)))
            return true;
            
        state.Label = label;
        label = $":{label};";

        for (var i = 0; i < state.Lines.Count; i++)
        {
            line = state.Lines[i];
            if (!line.ToUpperInvariant().StartsWith(label))
                continue;

            state.Index = i;
            return true;
        }

        return false;
    }
}