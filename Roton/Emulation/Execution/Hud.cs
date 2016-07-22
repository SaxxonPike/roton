using Roton.Core;

namespace Roton.Emulation.Execution
{
    internal abstract class Hud : IHud
    {
        protected Hud(IDisplayInfo infoSource)
        {
            DisplayInfo = infoSource;
        }

        protected int Ammo => DisplayInfo.Ammo;
        protected int Gems => DisplayInfo.Gems;
        protected IKeyList Keys => DisplayInfo.Keys;
        protected int TorchCycles => DisplayInfo.TorchCycles;
        protected int Torches => DisplayInfo.Torches;

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

        public IDisplayInfo DisplayInfo { get; }

        public virtual void DrawChar(int x, int y, AnsiChar ac)
        {
        }

        public virtual void DrawMessage(string message, int color)
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

        public virtual void RedrawBoard()
        {
        }

        public virtual int SelectParameter(bool performSelection, int x, int y, string message, int currentValue)
        {
            return currentValue;
        }

        public virtual ITerminal Terminal { get; set; }

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