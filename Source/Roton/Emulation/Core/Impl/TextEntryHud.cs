using System;
using System.Diagnostics;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class TextEntryHud : ITextEntryHud
{
    private readonly Lazy<ITerminal> _terminal;
    private readonly Lazy<IEngine> _engine;

    public TextEntryHud(Lazy<ITerminal> terminal, Lazy<IEngine> engine)
    {
        _terminal = terminal;
        _engine = engine;
    }

    private ITerminal Terminal
    {
        [DebuggerStepThrough] get => _terminal.Value;
    }

    private IEngine Engine
    {
        [DebuggerStepThrough] get => _engine.Value;
    }

    public string Show(int x, int y, int maxLength, int textColor, int pipColor)
    {
        var update = true;
        var cheat = string.Empty;
        var done = false;

        while (!done && Engine.ThreadActive)
        {
            if (update)
            {
                update = false;
                Terminal.Write(x, y, new string(' ', maxLength + 1), pipColor);
                Terminal.Plot(x + cheat.Length, y, new AnsiChar(0x1F, pipColor));
                Terminal.Write(x, y + 1, new string(' ', maxLength), textColor);
                Terminal.Write(x, y + 1, cheat, textColor);
            }
                
            Engine.WaitForTick();
            Engine.ReadInput();

            var key = Engine.State.KeyPressed;
            if (key == EngineKeyCode.None)
                continue;

            var keyChar = (int) key;
            if (keyChar is >= 0x20 and <= 0x7F)
            {
                if (cheat.Length < maxLength)
                {
                    cheat += (char) key;
                    update = true;
                }
            }
            else
            {
                switch (key)
                {
                    case EngineKeyCode.Left:
                    case EngineKeyCode.Backspace:
                        if (cheat.Length > 0)
                        {
                            cheat = cheat.Substring(0, cheat.Length - 1);
                            update = true;
                        }
                        break;
                    case EngineKeyCode.Enter:
                        done = true;
                        break;
                    case EngineKeyCode.Escape:
                        cheat = string.Empty;
                        done = true;
                        break;
                }
            }
        }

        for (var i = 0; i < 3; i++)
            Terminal.Write(x, y + i, new string(' ', maxLength + 1), pipColor);
        return cheat;
    }
}