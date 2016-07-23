namespace Roton.Core
{
    public interface IHud
    {
        ITerminal Terminal { get; set; }
        void ClearPausing();
        void ClearTitleStatus();
        bool Confirm(string message);
        void CreateStatusBar();
        void CreateStatusText();
        void DrawChar(int x, int y, AnsiChar ac);
        void DrawMessage(string message, int color);
        void DrawPausing();
        void DrawStatusLine(int y);
        void DrawString(int x, int y, string text, int color);
        void DrawTile(int x, int y, AnsiChar ac);
        void DrawTitleStatus();
        bool EndGameConfirmation();
        void FadeBoard(AnsiChar ac);
        void GenerateFadeMatrix();
        void RedrawBoard();
        int SelectParameter(bool performSelection, int x, int y, string message, int currentValue);
        void UpdateBorder();
        void UpdateCamera();
        void UpdateStatus();
    }
}