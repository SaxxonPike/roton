using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperHud(
    ITerminal terminal,
    IScroll scroll,
    ITextEntryHud textEntryHud,
    IFadeMatrix fadeMatrix,
    IState state,
    IBoard board,
    IActorList actors,
    ITiles tiles,
    IWorld world,
    IElementList elements,
    ISoundUnit soundUnit,
    IStatistics statistics,
    IDelayer delayer,
    IConfirmInputHandler confirmInputHandler)
    : IHud
{
    private readonly string _arrows = new([
        0x18.ToChar(),
        0x19.ToChar(),
        0x1A.ToChar(),
        0x1B.ToChar()
    ]);

    private readonly string _bottomOfHelp = new(0xDC.ToChar(), 12);

    private readonly string _topOfStatus = new(0xDF.ToChar(), 12);

    private const int ViewportHeight = 25;
    private const int ViewportWidth = 40;

    private const int WindowWidth = 24;
    private const int WindowHeight = 20;
    private const int WindowLeft = 14;
    private const int WindowTop = 2;
    private const int WindowRight = WindowLeft + WindowWidth - 1;
    private const int WindowBottom = WindowTop + WindowHeight - 1;

    private bool TitleScreen => state.PlayerElement != elements.PlayerId;

    private bool Confirm(string message)
    {
        UpdateBorder();
        DrawString(0x0F, 0x18, message, 0x1F);
        DrawChar(0x0F + message.Length, 0x18, new AnsiChar(0x5F, 0x9E));
        var result = confirmInputHandler.Confirm();
        UpdateBorder();
        return result;
    }

    public void CreateStatusBar()
    {
        for (var y = 0; y < ViewportHeight; y++)
        {
            for (var x = 0; x < ViewportWidth; x++)
            {
                if (x < WindowLeft || x > WindowRight || y < WindowTop || y > WindowBottom)
                    DrawChar(x, y, new AnsiChar(0x20, 0x1F));
            }
        }
    }

    public void ClearTitleStatus()
    {
    }

    public void CreateStatusText()
    {
        var buffer = (stackalloc char[16]);

        CreateStatusBar();
        if (TitleScreen)
        {
            DrawString(0x04, 0x0A, "Press", 0x1E);
            DrawString(0x04, 0x0C, "ENTER", 0x1F);
            DrawString(0x01, 0x0E, "to continue", 0x1E);
        }
        else
        {
            var barBuffer = buffer.Slice(0, 12);
            barBuffer.Fill(0xDC.ToChar());
            DrawString(0x00, 0x00, barBuffer, 0x1D);
            DrawString(0x00, 0x01, "  Commands  ", 0x6F);
            barBuffer.Fill(0xDF.ToChar());
            DrawString(0x00, 0x02, barBuffer, 0x6D);
            DrawString(0x00, 0x03, " ", _arrows, "       ", 0x6F);
            DrawString(0x00, 0x04, "   Move     ", 0x6E);
            DrawString(0x00, 0x05, " Shift+", _arrows, " ", 0x6F);
            DrawString(0x00, 0x06, "   Shoot    ", 0x6B);
            DrawString(0x00, 0x07, "   Hint     ", 0x6E);
            DrawString(0x01, 0x07, "H", 0x6F);
            DrawString(0x00, 0x08, "   Save Game", 0x6B);
            DrawString(0x01, 0x08, "S", 0x6F);
            DrawString(0x00, 0x09, "   Restore  ", 0x6E);
            DrawString(0x01, 0x09, "R", 0x6F);
            DrawString(0x00, 0x0A, "   Be Quiet ", 0x6E);
            DrawString(0x01, 0x0A, "B", 0x6F);
            DrawString(0x00, 0x0B, "   Quit     ", 0x6E);
            DrawString(0x01, 0x0B, "Q", 0x6F);
            DrawString(0x00, 0x0C, _bottomOfHelp, 0x1D);
            DrawString(0x00, 0x0D, "   Status   ", 0x6F);
            DrawString(0x00, 0x0E, _topOfStatus, 0x6D);
            DrawString(0x00, 0x0F, "Health      ", 0x6F);
            DrawString(0x00, 0x10, "            ", 0x6F);
            DrawString(0x00, 0x11, " Gems       ", 0x6F);
            DrawChar(0x06, 0x11, new AnsiChar(0x04, 0x62));
            DrawString(0x00, 0x12, " Ammo       ", 0x6F);
            DrawChar(0x06, 0x12, new AnsiChar(0x84, 0x6B));
            DrawString(0x00, 0x13, " Keys       ", 0x6F);
            DrawString(0x00, 0x14, "            ", 0x6F);
            DrawString(0x00, 0x15, " Score      ", 0x6F);
            DrawString(0x00, 0x16, "            ", 0x6F);
            DrawString(0x00, 0x17, "            ", 0x6F);
        }

        CreateStatusWindow();
    }

    private void CreateStatusWindow()
    {
        for (var x = 0; x < 26; x++)
            DrawChar(0x0D + x, 0x01, new AnsiChar(0xDC, 0x1F));
        DrawChar(0x0D, 0x16, new AnsiChar(0xDF, 0x1F));
        for (var x = 0; x < 25; x++)
            DrawChar(0x0E + x, 0x16, new AnsiChar(0xDF, 0x7F));

        for (var y = 0x02; y <= 0x15; y++)
        {
            DrawChar(0x0D, y, new AnsiChar(0xDB, 0x0F));
            DrawChar(0x26, y, new AnsiChar(0xDB, 0x0F));
            DrawChar(0x27, y + 1, new AnsiChar(0xDE, 0x71));
        }
    }

    public void DrawChar(int x, int y, AnsiChar ac) =>
        terminal.Plot(x, y, ac);

    public void DrawMessage(IMessage message, int color)
    {
        var topText = message.Text.FirstOrDefault() ?? string.Empty;
        var bottomText = message.Text.Skip(1).FirstOrDefault() ?? string.Empty;
        var topX = 26 - (topText.Length >> 1);
        var bottomX = 26 - (bottomText.Length >> 1);
        var messageColor = (color & 0x0F) | 0x10;
        DrawString(topX, 23, " ", topText, " ", messageColor);
        DrawString(bottomX, 24, " ", bottomText, " ", messageColor);
    }

    private void DrawSystemMessage(ReadOnlySpan<char> message, int color) =>
        DrawString(25 - message.Length / 2, 23, message, color);

    private void DrawNumber(int y, int value)
    {
        var buffer = (stackalloc char[6]);
        var s = value.ToCharSpan(buffer);
        var x = 11 - s.Length;
        DrawString(0x07, y, "   ", 0x6E);
        DrawString(x, y, s, 0x6E);
    }

    public void DrawString(int x, int y, ReadOnlySpan<char> text, int color) =>
        terminal.Write(x, y, text, color);

    private void DrawString(int x, int y, ReadOnlySpan<char> text0, ReadOnlySpan<char> text1, int color) =>
        terminal.Write(x, y, text0, text1, color);

    private void DrawString(int x, int y, ReadOnlySpan<char> text0, ReadOnlySpan<char> text1, ReadOnlySpan<char> text2,
        int color) =>
        terminal.Write(x, y, text0, text1, text2, color);

    public void Initialize()
    {
        RandomizeFadeMatrix();

        if (state.EditorMode)
            terminal.SetSize(96, 80, true);
        else
            terminal.SetSize(40, 25, true);
    }

    public bool QuitEngineConfirmation()
    {
        return Confirm("Quit to DOS? ");
    }

    public void RedrawBoard()
    {
        UpdateCameraPosition();
        fadeMatrix.FadeIn();
    }

    public IScrollState ShowScroll(bool isHelp, string? title, IEnumerable<string> lines) =>
        scroll.ShowMessage(title, lines, isHelp, 0);

    public void UpdateBorder() =>
        ClearMessage();

    private void UpdateCameraPosition()
    {
        var cameraX = actors.Player.Location.X - WindowWidth / 2;
        var cameraY = actors.Player.Location.Y - WindowHeight / 2;

        board.Camera = new Location16(
            Math.Max(Math.Min(cameraX, tiles.Width - WindowWidth + 1), 1),
            Math.Max(Math.Min(cameraY, tiles.Height - WindowHeight + 1), 1)
        );
    }


    public void UpdateStatus()
    {
        if (TitleScreen)
            return;

        if (world.Health < 0)
        {
            world.Health = 0;
        }

        var healthRemaining = (int)world.Health;
        for (var x = 7; x < 12; x++)
        {
            switch (healthRemaining)
            {
                case >= 20:
                    DrawChar(x, 0x0F, new AnsiChar(0xDB, 0x6E));
                    break;
                case >= 10:
                    DrawChar(x, 0x0F, new AnsiChar(0xDD, 0x6E));
                    break;
                default:
                    DrawChar(x, 0x0F, new AnsiChar(0x20, 0x6E));
                    break;
            }

            healthRemaining -= 20;
        }

        DrawNumber(0x11, world.Gems);
        DrawNumber(0x12, world.Ammo);
        DrawNumber(0x15, world.Score);
        DrawString(0x00, 0x16, "            ", 0x6F);

        var stoneText = StoneText;

        if (!string.IsNullOrWhiteSpace(stoneText))
        {
            DrawString(0x01, 0x16, stoneText, 0x6F);
        }

        if (world.Stones >= 0)
        {
            DrawNumber(0x16, world.Stones);
        }

        for (var i = 0; i < 7; i++)
        {
            var keyChar = world.Keys[i] ? (byte)elements.Key().Character : 0x20;
            var x = i & 0x3;
            var y = i >> 2;
            DrawChar(0x07 + x, 0x13 + y, new AnsiChar(keyChar, 0x69 + i));
        }

        DrawString(0x03, 0x0A, state.GameQuiet ? "Be Noisy " : "Be Quiet ", 0x6E);

        if (world.Flags.Contains("DEBUG"))
            DrawString(0x0E, 0x00, $"Used: {statistics.CalculateMemoryUsage()}", 0x1E);
    }

    public void CreateStatusWorld()
    {
    }

    private string StoneText
    {
        get
        {
            foreach (var flag in world.Flags.Select(f => f.ToUpperInvariant()))
            {
                if (flag.Length > 0 && flag.StartsWith("Z", StringComparison.OrdinalIgnoreCase))
                {
                    return flag.Substring(1);
                }
            }

            return string.Empty;
        }
    }

    public bool EndGameConfirmation() =>
        Confirm("End this game? ");

    public string EnterCheat()
    {
        UpdateBorder();
        var cheat = textEntryHud.Show(0x0F, 0x17, 11, 0x0F, 0x1F);
        UpdateBorder();
        return cheat;
    }

    public string SaveGame()
    {
        DrawString(13, 24, "Save game:", 0x1F);
        DrawString(33, 24, ".SAV", 0x0F);
        var result = textEntryHud.Show(25, 23, 8, 0x0F, 0x1F);
        UpdateBorder();
        return result;
    }

    public int SelectParameter(bool performSelection, int x, int y, string message, int currentValue,
        string? barText) =>
        currentValue;

    public IScrollState ShowHelp(string title, string fileName) =>
        scroll.ShowHelpFile(title, fileName);

    public void FadeBoard(AnsiChar ac) =>
        fadeMatrix.FadeOut(ac);

    private void RandomizeFadeMatrix() =>
        fadeMatrix.Randomize();

    public void FailToLoadWorld()
    {
        DrawSystemMessage("Wrong ZZT version!", 0x1E);
        soundUnit.PlayErrorSound();
        delayer.Delay(2000);
    }

    public void DrawPausing() =>
        DrawString(21, 24, "Pausing...", 0x1E);

    public void DrawTitleStatus()
    {
    }

    public void ClearPausing() =>
        ClearMessage();

    private void ClearMessage()
    {
        var clearChar = new AnsiChar(0x00, 0x10);
        for (var x = 12; x < 40; x++)
        {
            DrawChar(x, 23, clearChar);
            DrawChar(x, 24, clearChar);
        }
    }
}