using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton
{
    abstract public partial class Display
    {
        public Display(IDisplayInfo infoSource)
        {
            this.DisplayInfo = infoSource;
        }

        virtual public void ClearPausing() { }
        virtual public void ClearTitleStatus() { }
        virtual public bool Confirm(string message) { return true; }
        virtual public void CreateStatusBar() { }
        virtual public void CreateStatusText() { }
        public IDisplayInfo DisplayInfo { get; private set; }
        virtual public void DrawChar(int x, int y, AnsiChar ac) { }
        virtual public void DrawMessage(string message, int color) { }
        virtual public void DrawPausing() { }
        virtual public void DrawStatusLine(int y) { }
        virtual public void DrawString(int x, int y, string text, int color) { }
        virtual public void DrawTile(int x, int y, AnsiChar ac) { }
        virtual public void DrawTitleStatus() { }
        virtual public bool EndGameConfirmation() { return true; }
        virtual public void FadeBoard(AnsiChar ac) { }
        virtual public void GenerateFadeMatrix() { }
        virtual public void RedrawBoard() { }
        virtual public int SelectParameter(bool performSelection, int x, int y, string message, int currentValue) { return currentValue; }
        virtual public ITerminal Terminal { get; set; }
        virtual public void UpdateBorder() { }
        virtual public void UpdateStatus() { }
    }
}
