using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IHud
{
    void ClearPausing();
    void ClearTitleStatus();
    void CreateStatusText();
    void FailToLoadWorld();
    void DrawMessage(IMessage message, int color);
    void DrawPausing();
    void DrawTitleStatus();
    bool EndGameConfirmation();
    string EnterCheat();
    void FadeBoard(AnsiChar ac);
    void Initialize();
    bool QuitEngineConfirmation();
    void RedrawBoard();
    string SaveGame();
    int SelectParameter(bool performSelection, int x, int y, string message, int currentValue, string? barText);
    void UpdateBorder();
    void UpdateStatus();
    void CreateStatusWorld();
}