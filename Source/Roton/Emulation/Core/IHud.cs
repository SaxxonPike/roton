using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IHud
{
    void ClearPausing();
    void ClearTitleStatus();
    void CreateStatusText();
    void FailToLoadWorld();
    void DrawMessage(int color);
    void DrawPausing();
    void DrawTitleStatus();
    
    /// <remarks>
    /// RoZ: GamePromptEndPlay
    /// </remarks>
    bool EndGameConfirmation();
    string EnterCheat();

    /// <remarks>
    /// RoZ: TransitionDrawToFill
    /// </remarks>
    void FadeBoard(AnsiChar ac);

    void Initialize();
    bool QuitEngineConfirmation();

    /// <remarks>
    /// RoZ: TransitionDrawToBoard
    /// </remarks>
    void RedrawBoard();

    /// <remarks>
    /// RoZ: GameWorldSave (UI only)
    /// </remarks>
    string SaveGame();

    int SelectParameter(bool performSelection, int x, int y, string message, int currentValue, string? barText);

    /// <remarks>
    /// RoZ: BoardDrawBorder
    /// </remarks>
    void UpdateBorder();

    void UpdateStatus();
    void CreateStatusWorld();
}