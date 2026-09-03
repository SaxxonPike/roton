using System;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalHud(
    ITerminal terminal,
    ITextEntryHud textEntryHud,
    IChoiceHud choiceHud,
    IFadeMatrix fadeMatrix,
    IState state,
    IWorld world,
    IBoard board,
    IFacts facts,
    ISoundPlayer soundPlayer,
    IBoardUpdater boardUpdater,
    IPlayField playField,
    IElementList elements,
    IStatistics statistics,
    IDelayer delayer,
    IConfirmInputHandler confirmInputHandler)
    : IHud
{
    private const int ViewportHeight = 25;

    private const int ViewportWidth = 60;

    private bool TitleScreen => state.PlayerElement != elements.PlayerId;

    public void ClearPausing() =>
        DrawStatusLine(5);

    public void ClearTitleStatus() =>
        DrawStatusLine(6);

    private bool Confirm(string message)
    {
        DrawStatusLine(3);
        DrawStatusLine(4);
        DrawStatusLine(5);
        DrawString(0x3F, 0x05, message, 0x1F);
        DrawChar(0x3F + message.Length, 0x05, new AnsiChar(0x5F, 0x9E));
        var result = confirmInputHandler.Confirm();
        DrawStatusLine(5);
        return result;
    }

    private void CreateStatusBar()
    {
        for (var y = 0; y < ViewportHeight; y++)
        {
            DrawStatusLine(y);
        }
    }

    public void CreateStatusText()
    {
        CreateStatusBar();
        DrawStatusLine(0);
        DrawStatusLine(1);
        DrawStatusLine(2);
        DrawString(0x3D, 0, "    - - - - -      ", 0x1F);
        DrawString(0x3E, 1, "     Roton     ", 0x70);
        DrawString(0x3D, 2, "    - - - - -      ", 0x1F);

        if (TitleScreen)
        {
            SelectParameter(false, 0x42, 0x15, "Game speed:;FS", state.GameSpeed, null);
            DrawString(0x3E, 0x15, " S ", 0x70);
            DrawString(0x3E, 0x07, " W ", 0x30);
            DrawString(0x41, 0x07, " World:", 0x1E);
            CreateStatusWorld();
            DrawString(0x3E, 0x0B, " P ", 0x70);
            DrawString(0x41, 0x0B, " Play", 0x1F);
            DrawString(0x3E, 0x0C, " R ", 0x30);
            DrawString(0x41, 0x0C, " Restore game", 0x1E);
            DrawString(0x3E, 0x0D, " Q ", 0x70);
            DrawString(0x41, 0x0D, " Quit", 0x1E);
            DrawString(0x3E, 0x10, " A ", 0x30);
            DrawString(0x41, 0x10, " About Roton!", 0x1F);
            DrawString(0x3E, 0x11, " H ", 0x70);
            DrawString(0x41, 0x11, " High Scores", 0x1E);
            DrawString(0x3E, 0x12, " E ", 0x30);
            DrawString(0x41, 0x12, " Board Editor", 0x1E);
        }
        else
        {
            DrawString(0x40, 0x07, " Health:", 0x1E);
            DrawString(0x40, 0x08, "   Ammo:", 0x1E);
            DrawString(0x40, 0x09, "Torches:", 0x1E);
            DrawString(0x40, 0x0A, "   Gems:", 0x1E);
            DrawString(0x40, 0x0B, "  Score:", 0x1E);
            DrawString(0x40, 0x0C, "   Keys:", 0x1E);
            DrawChar(0x3E, 0x07, new AnsiChar(elements.Player().Character, 0x1F));
            DrawChar(0x3E, 0x08, new AnsiChar(elements.Ammo().Character, 0x1B));
            DrawChar(0x3E, 0x09, new AnsiChar(elements.Torch().Character, 0x16));
            DrawChar(0x3E, 0x0A, new AnsiChar(elements.Gem().Character, 0x1B));
            DrawChar(0x3E, 0x0C, new AnsiChar(elements.Key().Character, 0x1F));
            DrawString(0x3E, 0x0E, " T ", 0x70);
            DrawString(0x41, 0x0E, " Torch", 0x1F);
            DrawString(0x3E, 0x0F, " B ", 0x30);
            DrawString(0x3E, 0x10, " H ", 0x70);
            DrawString(0x41, 0x10, " Help", 0x1F);
            DrawChar(0x43, 0x12, new AnsiChar(0x20, 0x30));
            DrawChar(0x44, 0x12, new AnsiChar(0x18, 0x30));
            DrawChar(0x45, 0x12, new AnsiChar(0x19, 0x30));
            DrawChar(0x46, 0x12, new AnsiChar(0x1A, 0x30));
            DrawChar(0x47, 0x12, new AnsiChar(0x1B, 0x30));
            DrawChar(0x48, 0x12, new AnsiChar(0x20, 0x30));
            DrawString(0x48, 0x12, " Move", 0x1F);
            DrawChar(0x44, 0x13, new AnsiChar(0x18, 0x70));
            DrawChar(0x45, 0x13, new AnsiChar(0x19, 0x70));
            DrawChar(0x46, 0x13, new AnsiChar(0x1A, 0x70));
            DrawChar(0x47, 0x13, new AnsiChar(0x1B, 0x70));
            DrawChar(0x48, 0x13, new AnsiChar(0x20, 0x70));
            DrawString(0x3D, 0x13, " Shift ", 0x70);
            DrawString(0x48, 0x13, " Shoot", 0x1F);
            DrawString(0x3E, 0x15, " S ", 0x70);
            DrawString(0x41, 0x15, " Save game", 0x1F);
            DrawString(0x3E, 0x16, " P ", 0x30);
            DrawString(0x41, 0x16, " Pause", 0x1F);
            DrawString(0x3E, 0x17, " Q ", 0x70);
            DrawString(0x41, 0x17, " Quit", 0x1F);
        }
    }

    public void CreateStatusWorld()
    {
        DrawStatusLine(0x08);
        DrawString(0x45, 0x08,
            world.Name.Length <= 0 ? facts.UntitledWorldName : world.Name, 0x1F);
    }

    private void DrawChar(int x, int y, AnsiChar ac) =>
        terminal.Plot(x, y, ac);

    public void DrawMessage(IMessage message, int color)
    {
        var text = message.Text.FirstOrDefault();

        if (string.IsNullOrEmpty(text))
            return;

        var x = (60 - text.Length) / 2;
        DrawString(x, 24, " ", text, " ", color);
    }

    public void DrawPausing() =>
        DrawString(0x40, 0x05, "Pausing...", 0x1F);

    private void DrawStatusLine(int y)
    {
        var blankChar = new AnsiChar(0x20, 0x11);

        for (var x = 60; x < 80; x++)
            terminal.Plot(x, y, blankChar);
    }

    private void DrawString(int x, int y, ReadOnlySpan<char> text, int color) =>
        terminal.Write(x, y, text, color);

    private void DrawString(int x, int y, ReadOnlySpan<char> text0, ReadOnlySpan<char> text1, int color) =>
        terminal.Write(x, y, text0, text1, color);

    private void DrawString(int x, int y, ReadOnlySpan<char> text0, ReadOnlySpan<char> text1, ReadOnlySpan<char> text2,
        int color) =>
        terminal.Write(x, y, text0, text1, text2, color);

    private void DrawTileAt(Location location) =>
        DrawTileCommon(location.X, location.Y, boardUpdater.Draw(location + 1));

    private void DrawTileCommon(int x, int y, AnsiChar ac) =>
        playField.DrawTile(x, y, ac);

    public void DrawTitleStatus() =>
        DrawString(0x3E, 0x05, "Pick a command:", 0x1B);

    public void FadeBoard(AnsiChar ac) =>
        fadeMatrix.FadeOut(ac);

    private void RandomizeFadeMatrix() =>
        fadeMatrix.Randomize();

    public void Initialize()
    {
        RandomizeFadeMatrix();
        terminal.SetSize(state.EditorMode ? 60 : 80, 25, false);
    }

    public bool QuitEngineConfirmation() =>
        Confirm("Quit to DOS? ");

    public void RedrawBoard() =>
        fadeMatrix.FadeIn();

    public void UpdateBorder()
    {
        for (var x = 0; x < ViewportWidth; x++)
        {
            DrawTileAt(new Location(x, 0));
            DrawTileAt(new Location(x, ViewportHeight - 1));
        }

        for (var y = 0; y < ViewportHeight; y++)
        {
            DrawTileAt(new Location(0, y));
            DrawTileAt(new Location(ViewportWidth - 1, y));
        }
    }

    public void UpdateStatus()
    {
        var buffer = (stackalloc char[16]);

        if (TitleScreen)
            return;

        if (board.TimeLimit <= 0)
        {
            DrawStatusLine(6);
        }
        else
        {
            DrawString(0x40, 0x06, "   Time:", 0x1E);
            DrawString(0x48, 0x06, ((int)(board.TimeLimit - world.TimePassed)).ToCharSpan(buffer), 0x1E);
        }

        if (world.Health < 0)
        {
            world.Health = 0;
        }

        DrawString(0x48, 0x07, ((int)world.Health).ToCharSpan(buffer), " ", 0x1E);
        DrawString(0x48, 0x08, ((int)world.Ammo).ToCharSpan(buffer), " ", 0x1E);
        DrawString(0x48, 0x09, ((int)world.Torches).ToCharSpan(buffer), " ", 0x1E);
        DrawString(0x48, 0x0A, ((int)world.Gems).ToCharSpan(buffer), " ", 0x1E);
        DrawString(0x48, 0x0B, ((int)world.Score).ToCharSpan(buffer), " ", 0x1E);

        if (world.TorchCycles > 0)
        {
            for (var i = 2; i <= 5; i++)
            {
                DrawChar(0x49 + i, 0x09,
                    world.TorchCycles / 40 < i ? new AnsiChar(0xB0, 0x16) : new AnsiChar(0xB1, 0x16));
            }
        }
        else
        {
            DrawString(0x4B, 0x09, "    ", 0x16);
        }

        for (var i = 1; i <= 7; i++)
        {
            DrawChar(0x47 + i, 0x0C,
                world.Keys[i - 1]
                    ? new AnsiChar(elements.Key().Character, 0x18 + i)
                    : new AnsiChar(0x20, 0x1F));
        }

        DrawString(0x41, 0x0F, state.GameQuiet ? " Be noisy" : " Be quiet", 0x1F);

        if (world.Flags.Contains("DEBUG"))
            DrawString(0x3E, 0x04, "Used: ", statistics.CalculateMemoryUsage().ToCharSpan(buffer), 0x1E);
    }

    public string EnterCheat()
    {
        DrawStatusLine(4);
        DrawStatusLine(5);
        var cheat = textEntryHud.Show(0x3F, 0x04, 11, 0x0F, 0x1F, ReadOnlySpan<char>.Empty);
        DrawStatusLine(4);
        DrawStatusLine(5);
        return cheat;
    }

    public int SelectParameter(bool performSelection, int x, int y, string message, int currentValue,
        string? barText) =>
        choiceHud.Show(performSelection, x, y, message, currentValue, barText);

    public string SaveGame()
    {
        DrawString(65, 3, "Save game:", 0x1F);
        DrawString(71, 5, ".SAV", 0x0F);
        var result = textEntryHud.Show(63, 4, 8, 0x0F, 0x1F, state.DefaultSaveName);
        DrawStatusLine(3);
        DrawStatusLine(5);
        return result;
    }

    public void FailToLoadWorld()
    {
        DrawString(62, 4, "You need a newer", 0x1E);
        DrawString(62, 5, " version of ZZT!", 0x1E);
        soundPlayer.PlayErrorSound();
        delayer.Delay(2000);
    }

    public bool EndGameConfirmation() =>
        Confirm("End this game? ");
}