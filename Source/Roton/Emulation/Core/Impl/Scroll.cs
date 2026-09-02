using System;
using System.Collections.Generic;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Scroll(
    IState state,
    IFileSystem fileSystem,
    IScrollContent scrollContent,
    IScheduler scheduler,
    IInputReader inputReader,
    IGameThread gameThread,
    IFacts facts,
    IScrollBuffer scrollBuffer,
    IScrollRenderer scrollRenderer)
    : IScroll
{
    private ScrollResult MainLoop(ScrollState st)
    {
        var update = false;

        while (gameThread.ThreadActive)
        {
            if (update)
            {
                scrollRenderer.RenderContent(st);
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
            scrollRenderer.RenderContent(scrollState);
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
        scrollRenderer.Open();
        scrollRenderer.RenderContent(scrollState);
        var result = mainLoop(scrollState);
        scrollRenderer.Close();
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

    public ScrollResult ShowMessage(string? title, IEnumerable<string> message, bool isHelp, int index,
        Func<ScrollState, ScrollResult>? mainLoop = null)
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

        return Show(st, mainLoop ?? ShowLoop);
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