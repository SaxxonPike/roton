using System;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public class CheatHud : ICheatHud
    {
        private readonly Lazy<ITerminal> _terminal;
        private readonly Lazy<IEngine> _engine;

        public CheatHud(Lazy<ITerminal> terminal, Lazy<IEngine> engine)
        {
            _terminal = terminal;
            _engine = engine;
        }

        private ITerminal Terminal => _terminal.Value;
        private IEngine Engine => _engine.Value;
        
        public string Show(int x, int y)
        {
            var update = true;
            var cheat = string.Empty;
            var done = false;

            while (!done && Engine.ThreadActive)
            {
                if (update)
                {
                    update = false;
                    Terminal.Write(x, y, "            ", 0x1F);
                    Terminal.Plot(x + cheat.Length, y, new AnsiChar(0x1F, 0x1F));
                    Terminal.Write(x, y + 1, "           ", 0x0F);
                    Terminal.Write(x, y + 1, cheat, 0x0F);
                }
                
                Engine.WaitForTick();
                Engine.ReadInput();

                var key = Engine.State.KeyPressed;
                if (key == EngineKeyCode.None)
                    continue;

                var keyChar = (int) key;
                if (keyChar >= 0x20 && keyChar <= 0x7F)
                {
                    if (cheat.Length < 11)
                    {
                        cheat = cheat + (char) key;
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
                Terminal.Write(x, y + i, "            ", 0x1F);
            return cheat;
        }
    }
}