using System;
using System.Diagnostics;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class TextEntryHud(
    ITerminal terminal,
    IEngineAccessor engine,
    IState state,
    IScheduler scheduler,
    IInputReader inputReader)
    : ITextEntryHud
{
    private IEngine Engine
    {
        [DebuggerStepThrough] get => engine.Instance;
    }

    public string Show(int x, int y, int maxLength, int textColor, int pipColor, ReadOnlySpan<char> initText = default)
    {
        var chars = (stackalloc char[maxLength]);
        initText.CopyTo(chars);
        var length = initText.Length;
        var update = true;
        var done = false;

        while (!done && Engine.ThreadActive)
        {
            if (update)
            {
                update = false;
                terminal.Write(x, y, new string(' ', maxLength + 1), pipColor);
                terminal.Plot(x + length, y, new AnsiChar(0x1F, pipColor));
                terminal.Write(x, y + 1, new string(' ', maxLength), textColor);
                terminal.Write(x, y + 1, chars, textColor);
            }

            scheduler.WaitForTick();
            inputReader.Read(true);

            var key = state.KeyPressed;
            if (key == EngineKeyCode.None)
                continue;

            var keyChar = (int)key;
            if (keyChar is >= 0x20 and <= 0x7F)
            {
                if (length < maxLength)
                {
                    chars[length] = (char)key;
                    length++;
                    update = true;
                }
            }
            else
            {
                switch (key)
                {
                    case EngineKeyCode.Left:
                    case EngineKeyCode.Backspace:
                        if (length > 0)
                        {
                            length--;
                            chars[length] = ' ';
                            update = true;
                        }

                        break;
                    case EngineKeyCode.Enter:
                        done = true;
                        break;
                    case EngineKeyCode.Escape:
                        length = 0;
                        done = true;
                        break;
                }
            }
        }

        for (var i = 0; i < 3; i++)
            terminal.Write(x, y + i, new string(' ', maxLength + 1), pipColor);

        return chars.Slice(0, length).ToString();
    }
}