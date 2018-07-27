using System;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Core.Impl
{
    [Context(Context.Original)]
    [Context(Context.Super)]
    public class ChoiceHud : IChoiceHud
    {
        private readonly Lazy<ITerminal> _terminal;
        private readonly Lazy<IEngine> _engine;

        public ChoiceHud(Lazy<ITerminal> terminal, Lazy<IEngine> engine)
        {
            _terminal = terminal;
            _engine = engine;
        }

        private ITerminal Terminal => _terminal.Value;
        private IEngine Engine => _engine.Value;
        
        private void DrawChar(int x, int y, AnsiChar ac)
        {
            Terminal.Plot(x, y, ac);
        }

        private void DrawStatusLine(int x, int y)
        {
            var blankChar = new AnsiChar(0x20, 0x11);
            for (var i = 0; i < 14; i++)
            {
                Terminal.Plot(x + i, y, blankChar);
            }            
        }

        private void DrawString(int x, int y, string message, int color)
        {
            Terminal.Write(x, y, message, color);
        }
        
        public int Show(bool performSelection, int x, int y, string message, int currentValue, string barText)
        {
            void DrawPip(int col)
            {
                for (var x2 = 0; x2 < 8; x2++)
                    DrawChar(x + x2 + 1, y + 1, new AnsiChar(0x20, col));
                
                if (barText == null)
                {
                    DrawChar(x + currentValue + 1,
                        y + 1,
                        new AnsiChar(0x1F, col));
                }
                else
                {
                    DrawChar(x + ((currentValue & 0x80) != 0 ? 0 : barText.IndexOf(' ') + 1) + 1,
                        y + 1, 
                        new AnsiChar(0x1F, 0x1F));
                }
            }
            
            DrawStatusLine(x, y);
            DrawStatusLine(x, y + 1);
            DrawStatusLine(x, y + 2);
            
            if (barText == null)
            {
                string minIndicator;
                string maxIndicator;
                if (message.Length >= 3 && message[message.Length - 3] == ';')
                {
                    minIndicator = message[message.Length - 2].ToString();
                    maxIndicator = message[message.Length - 1].ToString();
                    message = message.Substring(0, message.IndexOf(';') - 1);
                }
                else
                {
                    minIndicator = @"1";
                    maxIndicator = @"9";
                }

                DrawString(x, y, message, performSelection ? 0x1F : 0x1E);
                DrawString(x, y + 2, minIndicator + @"....:...." + maxIndicator, 0x1E);
            }
            else
            {
                DrawString(x, y + 2, barText, 0x1E);
            }

            if (!performSelection)
            {
                DrawPip(0x1F);
                return currentValue;
            }
            
            DrawPip(0x9F);
            
            while (Engine.ThreadActive)
            {
                var update = false;
                    
                Engine.ReadInput();

                switch (Engine.State.KeyPressed)
                {
                    case EngineKeyCode.None:
                        Engine.WaitForTick();
                        continue;
                    case EngineKeyCode.Left:
                        if (barText == null)
                        {
                            if (currentValue > 0)
                                currentValue--;
                        }
                        else
                        {
                            if (currentValue > 0)
                                currentValue = 0;
                        }
                        update = true;
                        break;
                    case EngineKeyCode.Right:
                        if (barText == null)
                        {
                            if (currentValue < 8)
                                currentValue++;
                        }
                        else
                        {
                            if (currentValue < 0x80)
                                currentValue = 0x80;
                        }
                        update = true;
                        break;
                }

                if (update)
                {
                    DrawStatusLine(x, y + 1);
                    DrawPip(0x9F);
                }
                    
                if (Engine.State.KeyShift || Engine.State.KeyPressed == EngineKeyCode.Enter ||
                    Engine.State.KeyPressed == EngineKeyCode.Escape)
                {
                    break;
                }
                
                Engine.WaitForTick();
            }

            DrawString(x, y, message, 0x1E);
            DrawPip(0x1F);
            
            return currentValue;
        }
    }
}