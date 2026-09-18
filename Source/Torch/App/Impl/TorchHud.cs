using Roton.Emulation.Core;
using Roton.Emulation.Data;

namespace Torch.App.Impl;

public class TorchHud(IFadeMatrix fadeMatrix) : IHud
{
    public void ClearPausing()
    {
    }

    public void ClearTitleStatus()
    {
    }

    public void CreateStatusText()
    {
    }

    public void FailToLoadWorld()
    {
    }

    public void DrawMessage(int color)
    {
    }

    public void DrawPausing()
    {
    }

    public void DrawTitleStatus()
    {
    }

    public bool EndGameConfirmation()
    {
        return false;
    }

    public string EnterCheat()
    {
        return string.Empty;
    }

    public void FadeBoard(AnsiChar ac)
    {
    }

    public void Initialize()
    {
    }

    public bool QuitEngineConfirmation()
    {
        return false;
    }

    public void RedrawBoard()
    {
        fadeMatrix.FadeIn();
    }

    public string SaveGame()
    {
        return string.Empty;
    }

    public int SelectParameter(bool performSelection, int x, int y, string message, int currentValue, string? barText)
    {
        return 0;
    }

    public void UpdateBorder()
    {
    }

    public void UpdateStatus()
    {
    }

    public void CreateStatusWorld()
    {
    }
}