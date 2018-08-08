using System.Collections.Generic;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;

namespace Roton.Emulation.Core.Impl
{
    public abstract class Hud : IHud
    {
        private readonly IEngine _engine;
        private readonly IScroll _scroll;

        protected Hud(IEngine engine, IScroll scroll)
        {
            _engine = engine;
            _scroll = scroll;
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
            return string.Empty;
        }

        public virtual void FadeBoard(AnsiChar ac)
        {
        }

        public abstract void Initialize();

        public virtual bool QuitEngineConfirmation()
        {
            return Confirm("Quit to DOS? ");
        }

        public virtual void RedrawBoard()
        {
        }

        public virtual string SaveGame()
        {
            // TODO: Save game hud
            return string.Empty;
        }

        public virtual int SelectParameter(bool performSelection, int x, int y, string message, int currentValue, string barText)
        {
            return currentValue;
        }

        public IScrollState ShowHelp(string title, string fileName) => 
            _scroll.Show(title, fileName);

        public IScrollState ShowScroll(bool isHelp, string title, IEnumerable<string> lines) =>
            _scroll.Show(title, lines, isHelp, 0);

        public virtual void UpdateBorder()
        {
        }

        public virtual void UpdateCamera()
        {
        }

        public virtual void UpdateStatus()
        {
        }

        public virtual void CreateStatusWorld()
        {
        }

        public virtual string EnterHighScore(int score)
        {
            return null;
        }

        protected virtual bool Confirm(string message)
        {
            while (_engine.ThreadActive)
            {
                _engine.WaitForTick();
                _engine.ReadInput();
                switch (_engine.State.KeyPressed.ToUpperCase())
                {
                    case EngineKeyCode.Y:
                        return true;
                    case EngineKeyCode.N:
                    case EngineKeyCode.Escape:
                        return false;
                }
            }

            return true;
        }
    }
}