﻿using System.Collections.Generic;
using Roton.Core;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Execution
{
    public abstract class Hud : IHud
    {
        private readonly IEngine _engine;

        protected Hud(IEngine engine)
        {
            _engine = engine;
        }

        public virtual void ClearPausing()
        {
        }

        public virtual void ClearTitleStatus()
        {
        }

        public virtual void CreateStatusBar()
        {
        }

        public virtual void CreateStatusText()
        {
        }

        public virtual void DrawChar(int x, int y, AnsiChar ac)
        {
        }

        public virtual void DrawMessage(IMessage message, int color)
        {
        }

        public virtual void DrawPausing()
        {
        }

        public virtual void DrawStatusLine(int y)
        {
        }

        public virtual void DrawString(int x, int y, string text, int color)
        {
        }

        public virtual void DrawTile(int x, int y, AnsiChar ac)
        {
        }

        public virtual void DrawTitleStatus()
        {
        }

        public virtual bool EndGameConfirmation()
        {
            return Confirm("End this game? ");
        }

        public virtual string EnterCheat()
        {
            // TODO: cheat hud
            return string.Empty;
        }

        public virtual void FadeBoard(AnsiChar ac)
        {
        }

        public abstract void Initialize();

        public virtual bool QuitZztConfirmation()
        {
            return Confirm("Quit ZZT? ");
        }

        public virtual void RedrawBoard()
        {
        }

        public virtual string SaveGame()
        {
            // TODO: Save game hud
            return string.Empty;
        }

        public virtual int SelectParameter(bool performSelection, int x, int y, string message, int currentValue)
        {
            return currentValue;
        }

        public virtual IScrollResult ShowScroll(IEnumerable<string> lines)
        {
            // TODO: Actually implement scroll window
            
            // Fallback scroll implementation
            //MessageBox.Show(string.Join(Environment.NewLine, lines));
            return new ScrollResult {SelectedLine = -1};
        }

        public virtual void UpdateBorder()
        {
        }

        public virtual void UpdateCamera()
        {
        }

        public virtual void UpdateStatus()
        {
        }

        protected virtual bool Confirm(string message)
        {
            while (true)
            {
                _engine.WaitForTick();
                var key = _engine.ReadKey().ToUpperCase();
                switch (key)
                {
                    case 0x59:
                        return true;
                    case 0x4E:
                    case 0x1B:
                        return false;
                }
            }
        }
    }
}