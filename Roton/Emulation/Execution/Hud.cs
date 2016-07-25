using Roton.Core;

namespace Roton.Emulation.Execution
{
    internal abstract class Hud : IHud
    {
        protected Hud(IEngine engine, ITerminal terminal)
        {
            Engine = engine;
            Terminal = terminal;
        }

        protected IEngine Engine { get; set; }

        public virtual void ClearPausing()
        {
        }

        public virtual void ClearTitleStatus()
        {
        }

        public virtual bool Confirm(string message)
        {
            return true;
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
            return true;
        }

        public virtual void FadeBoard(AnsiChar ac)
        {
        }

        public virtual void GenerateFadeMatrix()
        {
        }

        public abstract void Initialize();

        public virtual void RedrawBoard()
        {
        }

        public virtual int SelectParameter(bool performSelection, int x, int y, string message, int currentValue)
        {
            return currentValue;
        }

        protected ITerminal Terminal { get; }

        public virtual void UpdateBorder()
        {
        }

        public virtual void UpdateCamera()
        {
        }

        public virtual void UpdateStatus()
        {
        }
    }
}