using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Core
{
    public interface IHud
    {
        void ClearPausing();
        void ClearTitleStatus();
        void CreateStatusBar();
        void CreateStatusText();
        void DrawChar(int x, int y, AnsiChar ac);
        void DrawMessage(IMessage message, int color);
        void DrawPausing();
        void DrawStatusLine(int y);
        void DrawString(int x, int y, string text, int color);
        void DrawTile(int x, int y, AnsiChar ac);
        void DrawTitleStatus();
        bool EndGameConfirmation();
        string EnterCheat();
        void FadeBoard(AnsiChar ac);
        void Initialize();
        bool QuitEngineConfirmation();
        void RedrawBoard();
        string SaveGame();
        int SelectParameter(bool performSelection, int x, int y, string message, int currentValue, string barText);
        IScrollResult ShowScroll(string title, params string[] lines);
        void UpdateBorder();
        void UpdateCamera();
        void UpdateStatus();
    }
}