using System;
using System.Collections.Generic;
using Roton.Emulation.Data;
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

    private void RenderContent(ScrollState scrollState)
    {
        var offset = scrollState.Index;
        var title = scrollState.Title;
        var buffer = (stackalloc char[256]);

        var center = (facts.ScrollHeight - 4) / 2;
        var line = offset - center;
        var bottom = facts.ScrollHeight + facts.ScrollTop - 2;
        var top = facts.ScrollTop + 3;
        var lineCount = scrollContent.LineCount;
        var y = top;

        RenderBlank(facts.ScrollTop + 1);

        for (var y2 = y; y2 <= bottom; y2++)
            RenderBlank(y2);

        RenderPips(top + center);

        while (y <= bottom)
        {
            if (scrollState.IsHelp)
            {
                switch (line)
                {
                    case -5:
                    {
                        terminal.Write(facts.ScrollLeft + 5, y, "Use            to view text,", 0x1A);
                        terminal.Write(facts.ScrollLeft + 9, y, "\u2191 \u2193, Enter", 0x1F);
                        break;
                    }
                    case -4:
                    {
                        terminal.Write(facts.ScrollLeft + 20, y, "to print.", 0x1A);
                        terminal.Write(facts.ScrollLeft + 14, y, "Alt-P", 0x1F);
                        break;
                    }
                }
            }

            if (line >= 0 && line < lineCount)
            {
                scrollContent.GetLine(line, buffer);
                RenderText(scrollContent.GetLine(line, buffer), y);
            }
            else if (line == -1 || line == lineCount)
            {
                RenderDots(y);
            }

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

    private ScrollResult MainLoop(ScrollState st)
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
                    return new ScrollResult(st.Index, st.Label, true, true);
                case EngineKeyCode.Enter:
                    return new ScrollResult(st.Index, st.Label, false, true);
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

        return new ScrollResult(st.Index, st.Label, true, true);
    }

    private bool LoadHelpFile(string filename)
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
        return true;
    }

    private ScrollResult ShowLoop(ScrollState scrollState)
    {
        while (true)
        {
            RenderContent(scrollState);
            var result = MainLoop(scrollState);

            state.CancelScroll = result.Cancelled;

            if (result.Cancelled)
                return result;

            var innerJump = SelectLine(result.Index, out var jumpLabel, out var jumpIndex);
            if (!innerJump)
                return result;

            scrollState.Index = jumpIndex;
            scrollState.Label = jumpLabel;
        }
    }

    private ScrollResult Show(ScrollState scrollState, Func<ScrollState, ScrollResult> mainLoop)
    {
        scrollBuffer.Capture();
        Open();
        RenderContent(scrollState);
        var result = mainLoop(scrollState);
        Close();
        return result;
    }

    public ScrollResult ShowHelpFile(string? title, string fileName)
    {
        var st = new ScrollState
        {
            Index = 0,
            Label = null,
            IsHelp = true,
            Title = title
        };

        if (LoadHelpFile(fileName))
            return Show(st, ShowLoop);

        return default;
    }

    public ScrollResult ShowMessage(string? title, IEnumerable<string> message, bool isHelp, int index)
        => ShowMessage(title, message, isHelp, index, ShowLoop);

    public ScrollResult ShowMessage(string? title, IEnumerable<string> message, bool isHelp, int index,
        Func<ScrollState, ScrollResult> mainLoop)
    {
        var st = new ScrollState
        {
            Index = index,
            Label = null,
            IsHelp = isHelp,
            Title = title
        };

        scrollContent.ClearLines();
        scrollContent.AddLines(message);

        return Show(st, mainLoop);
    }

    public int TextWidth => facts.ScrollWidth - 4;

    public int TextHeight => facts.ScrollHeight - 4;

    private bool SelectLine(int index, out string? newLabel, out int newIndex)
    {
        newLabel = null;
        newIndex = index;

        if (index < 0)
            return false;

        var buffer = (stackalloc char[256]);
        var line = scrollContent.GetLine(index, buffer);

        if (line.Length == 0 || line[0] != '!' || line.IndexOf(';') < 0)
            return false;

        var label = line
            .Slice(1, line.IndexOf(';') - 1)
            .ToString()
            .ToUpperInvariant();

        if (line[0] == '!' && label.Length > 0 && label[0] == '-' && LoadHelpFile(label.Substring(1)))
        {
            newIndex = 0;
            return true;
        }

        newLabel = label;
        label = $":{label};";
        var lineCount = scrollContent.LineCount;

        for (var i = 0; i < lineCount; i++)
        {
            line = scrollContent.GetLine(i, buffer);
            if (!line.StartsWith(label, StringComparison.OrdinalIgnoreCase))
                continue;

            newIndex = i;
            return true;
        }

        return false;
    }
}