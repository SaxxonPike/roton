using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IHud
{
    void ClearPausing();
    void ClearTitleStatus();
    void CreateStatusBar();
    void CreateStatusText();
    void DrawChar(int x, int y, AnsiChar ac);
    void FailToLoadWorld();
    void DrawMessage(IMessage message, int color);
    void DrawPausing();
    void DrawStatusLine(int y);
    void DrawTile(int x, int y, AnsiChar ac);
    void DrawTitleStatus();
    bool EndGameConfirmation();
    string EnterCheat();
    void FadeBoard(AnsiChar ac);
    void Initialize();
    bool QuitEngineConfirmation();
    void RedrawBoard();
    string SaveGame();
    int SelectParameter(bool performSelection, int x, int y, string message, int currentValue, string? barText);
    IScrollState ShowHelp(string title, string fileName);
    IScrollState ShowScroll(bool isHelp, string? title, IEnumerable<string> lines);
    void UpdateBorder();
    void UpdateCamera();
    void UpdateStatus();
    void CreateStatusWorld();
    string? EnterHighScore(IHighScoreList highScoreList, int score);
    void ShowHighScores(IHighScoreList highScoreList);
}