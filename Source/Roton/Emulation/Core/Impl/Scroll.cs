using System;
using System.Collections.Generic;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Scroll(
    ITerminal terminal,
    IState state,
    IFileSystem fileSystem,
    IScrollContent scrollContent,
    IScheduler scheduler,
    IInputReader inputReader,
    IGameThread gameThread,
    IFacts facts,
    IScrollBuffer scrollBuffer)
    : IScroll
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

    private void RenderLine(int[] chars, int y)
    {
        terminal.Plot(facts.ScrollLeft, y, new AnsiChar(chars[0], 0x0F));
        terminal.Plot(facts.ScrollLeft + 1, y, new AnsiChar(chars[1], 0x0F));
        for (var x = facts.ScrollLeft + 2; x < facts.ScrollLeft + facts.ScrollWidth - 2; x++)
            terminal.Plot(x, y, new AnsiChar(chars[2], 0x0F));
        terminal.Plot(facts.ScrollLeft + facts.ScrollWidth - 2, y, new AnsiChar(chars[3], 0x0F));
        terminal.Plot(facts.ScrollLeft + facts.ScrollWidth - 1, y, new AnsiChar(chars[4], 0x0F));
    }

    private void Open()
    {
        for (var y = facts.ScrollHeight / 2; y >= 0; y--)
        {
            var topY = facts.ScrollTop + y;
            var bottomY = facts.ScrollTop + facts.ScrollHeight - y - 1;

            RenderLine(ScrollCharsTop, topY);
            RenderLine(ScrollCharsBottom, bottomY);

            for (var y2 = topY + 1; y2 < bottomY - 1; y2++)
                RenderLine(ScrollCharsMid, y2);

            scheduler.WaitForTick();
        }

        RenderLine(ScrollCharsMid, facts.ScrollTop + facts.ScrollHeight - 2);
        RenderLine(ScrollCharsSplit, facts.ScrollTop + 2);
    }

    private void Close()
    {
        for (var y = 0; y < facts.ScrollHeight / 2; y++)
        {
            var topY = facts.ScrollTop + y;
            var bottomY = facts.ScrollTop + facts.ScrollHeight - y - 1;

            RenderLine(ScrollCharsTop, topY + 1);
            RenderLine(ScrollCharsBottom, bottomY - 1);
            scrollBuffer.Restore(topY);
            scrollBuffer.Restore(bottomY);

            scheduler.WaitForTick();
        }

        scrollBuffer.Restore(facts.ScrollTop + facts.ScrollHeight / 2);
    }

    private void RenderName(ReadOnlySpan<char> title, int offset)
    {
        var buffer = (stackalloc char[256]);
        var line = scrollContent.GetLine(offset, buffer);
        var pips = false;

        if (line.Length > 0 && (line[0] == ':' || line[0] == '!'))
        {
            title = "Press ENTER to select this";
            pips = true;
        }

        var x = facts.ScrollLeft + facts.ScrollWidth / 2 - title.Length / 2;
        terminal.Write(x, facts.ScrollTop + 1, title, 0x1E);

        // Avoid putting these directly in the string for Unicode conversion reasons
        if (pips)
        {
            terminal.Plot(x - 1, facts.ScrollTop + 1, new AnsiChar(0xAE, 0x1E));
            terminal.Plot(x + title.Length, facts.ScrollTop + 1, new AnsiChar(0xAF, 0x1E));
        }
    }

    private void RenderBlank(int y)
    {
        var x = facts.ScrollLeft + 2;
        var right = facts.ScrollLeft + facts.ScrollWidth - 3;
        var blank = new AnsiChar(0x20, 0x1E);

        for (var x2 = x; x2 <= right; x2++)
            terminal.Plot(x2, y, blank);
    }

    private void RenderPips(int y)
    {
        terminal.Plot(facts.ScrollLeft + 2, y, new AnsiChar(0xAF, 0x1C));
        terminal.Plot(facts.ScrollLeft + facts.ScrollWidth - 3, y, new AnsiChar(0xAE, 0x1C));
    }

    private void RenderText(ReadOnlySpan<char> text, int y)
    {
        var x = facts.ScrollLeft + 4;
        if (text.Length < 1)
            return;

        switch (text[0])
        {
            case '$':
            {
                var actualText = text.Slice(1);
                terminal.Write(facts.ScrollLeft + facts.ScrollWidth / 2 - actualText.Length / 2, y, actualText, 0x1F);
                break;
            }
            case ':':
            {
                if (text.IndexOf(';') >= 0)
                {
                    var actualText = text.Slice(text.IndexOf(';') + 1);
                    terminal.Write(x, y, actualText, 0x1F);
                }

                break;
            }
            case '!':
            {
                var actualText = text.Slice(text.IndexOf(';') + 1);
                terminal.Plot(facts.ScrollLeft + 4, y, new AnsiChar(0x10, 0x1D));
                terminal.Write(facts.ScrollLeft + 6, y, actualText, 0x1F);
                break;
            }
            default:
            {
                terminal.Write(x, y, text, 0x1E);
                break;
            }
        }
    }

    private void RenderContent(IScrollState scrollState)
    {
        var offset = scrollState.Index;
        var message = scrollContent;
        var title = scrollState.Title;
        var buffer = (stackalloc char[256]);

        var center = (facts.ScrollHeight - 4) / 2;
        var line = offset - center;
        var bottom = facts.ScrollHeight + facts.ScrollTop - 2;
        var top = facts.ScrollTop + 3;
        var lineCount = message.LineCount;
        var y = top;

        RenderBlank(facts.ScrollTop + 1);

        for (var y2 = y; y2 <= bottom; y2++)
            RenderBlank(y2);

        RenderPips(top + center);

        while (y <= bottom)
        {
            if (scrollState.IsHelp)
            {
                if (line == -5)
                {
                    terminal.Write(facts.ScrollLeft + 5, y, "Use            to view text,", 0x1A);
                    terminal.Write(facts.ScrollLeft + 9, y, "\u2191 \u2193, Enter", 0x1F);
                }
                else if (line == -4)
                {
                    terminal.Write(facts.ScrollLeft + 20, y, "to print.", 0x1A);
                    terminal.Write(facts.ScrollLeft + 14, y, "Alt-P", 0x1F);
                }
            }

            if (line >= 0 && line < lineCount)
            {
                message.GetLine(line, buffer);
                RenderText(message.GetLine(line, buffer), y);
            }
            else if (line == -1 || line == lineCount)
                RenderDots(y);

            y++;
            line++;
        }

        RenderName(title ?? "", offset);
    }

    private void RenderDots(int y)
    {
        var x = facts.ScrollLeft + 6;
        var right = facts.ScrollLeft + facts.ScrollWidth - 3;
        var dot = new AnsiChar(0x07, 0x1E);

        for (var x2 = x; x2 <= right; x2 += 5)
            terminal.Plot(x2, y, dot);
    }

    private bool MainLoop(IScrollState st)
    {
        var update = false;

        while (gameThread.ThreadActive)
        {
            if (update)
            {
                RenderContent(st);
                update = false;
            }

            inputReader.Read(true);

            switch (state.KeyPressed)
            {
                case EngineKeyCode.Escape:
                    return false;
                case EngineKeyCode.Enter:
                    return true;
                case EngineKeyCode.PageUp:
                    st.Index -= facts.ScrollHeight - 5;
                    update = true;
                    break;
                case EngineKeyCode.PageDown:
                    st.Index += facts.ScrollHeight - 5;
                    update = true;
                    break;
                case EngineKeyCode.Up:
                    st.Index--;
                    update = true;
                    break;
                case EngineKeyCode.Down:
                    st.Index++;
                    update = true;
                    break;
            }

            if (update)
            {
                if (st.Index >= scrollContent.LineCount)
                    st.Index = scrollContent.LineCount - 1;
                if (st.Index < 0)
                    st.Index = 0;
            }

            scheduler.WaitForTick();
        }

        return false;
    }

    private bool LoadHelpFile(IScrollState scrollState, string filename)
    {
        var text = fileSystem
            .GetFile($"{filename}.HLP")?
            .ToStringValue()
            .Replace("\r\n", "\r")
            .Split('\r');

        if (text == null)
            return false;

        scrollContent.ClearLines();
        scrollContent.AddLines(text);
        scrollState.Index = 0;
        scrollState.IsHelp = true;
        return true;
    }

    private void ShowLoop(IScrollState scrollState)
    {
        while (true)
        {
            RenderContent(scrollState);
            var selected = MainLoop(scrollState);
            if (!selected)
            {
                scrollState.Cancelled = true;
                break;
            }

            var innerJump = SelectLine(scrollState);
            if (!innerJump)
                break;
        }
    }

    private IScrollState Show(ScrollState scrollState, Action<ScrollState> mainLoop)
    {
        scrollBuffer.Capture();
        Open();
        RenderContent(scrollState);
        mainLoop(scrollState);
        Close();
        return scrollState;
    }

    public IScrollState Show(string title, string fileName)
    {
        var st = new ScrollState(state)
        {
            Index = 0,
            Label = null,
            Cancelled = false,
            IsHelp = true,
            Title = title
        };

        if (LoadHelpFile(st, fileName))
            return Show(st, ShowLoop);

        return st;
    }

    public IScrollState Show(string? title, IEnumerable<string> message, bool isHelp, int index)
        => Show(title, message, isHelp, index, ShowLoop);

    public IScrollState Show(string? title, IEnumerable<string> message, bool isHelp, int index,
        Action<IScrollState> mainLoop)
    {
        var st = new ScrollState(state)
        {
            Index = index,
            Label = null,
            Cancelled = false,
            IsHelp = isHelp,
            Title = title
        };

        scrollContent.ClearLines();
        scrollContent.AddLines(message);

        return Show(st, mainLoop);
    }

    public int TextWidth => facts.ScrollWidth - 4;

    public int TextHeight => facts.ScrollHeight - 4;

    private bool SelectLine(IScrollState scrollState)
    {
        if (scrollState.Index < 0)
            return false;

        var buffer = (stackalloc char[256]);
        var line = scrollContent.GetLine(scrollState.Index, buffer);

        if (line.Length == 0 || line[0] != '!' || line.IndexOf(';') < 0)
            return false;

        var label = line
            .Slice(1, line.IndexOf(';') - 1)
            .ToString()
            .ToUpperInvariant();

        if (line[0] == '!' && label.Length > 0 && label[0] == '-' && LoadHelpFile(scrollState, label.Substring(1)))
            return true;

        scrollState.Label = label;
        label = $":{label};";
        var lineCount = scrollContent.LineCount;

        for (var i = 0; i < lineCount; i++)
        {
            line = scrollContent.GetLine(i, buffer);
            if (!line.StartsWith(label, StringComparison.OrdinalIgnoreCase))
                continue;

            scrollState.Index = i;
            return true;
        }

        return false;
    }
}