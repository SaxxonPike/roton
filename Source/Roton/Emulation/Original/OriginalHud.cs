using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalHud(
    IEngineAccessor engine,
    ITerminal terminal,
    IScroll scroll,
    ITextEntryHud textEntryHud,
    IChoiceHud choiceHud,
    ILongTextEntryHud longTextEntryHud,
    IFadeMatrix fadeMatrix,
    IState state,
    IElementList elementList,
    IWorld world,
    IBoard board,
    IFacts facts,
    ISoundUnit soundUnit,
    IBoardUpdater boardUpdater,
    IPlayField playField,
    IScheduler scheduler,
    IInputReader inputReader,
    IElementList elements)
    : Hud(engine, scroll, state, scheduler, inputReader)
{
    private const int ViewportHeight = 25;

    private const int ViewportWidth = 60;

    private bool TitleScreen => State.PlayerElement != elements.PlayerId;

    public override void ClearPausing() => 
        DrawStatusLine(5);

    public override void ClearTitleStatus() => 
        DrawStatusLine(6);

    protected override bool Confirm(string message)
    {
        DrawStatusLine(3);
        DrawStatusLine(4);
        DrawStatusLine(5);
        DrawString(0x3F, 0x05, message, 0x1F);
        DrawChar(0x3F + message.Length, 0x05, new AnsiChar(0x5F, 0x9E));
        var result = base.Confirm(message);
        DrawStatusLine(5);
        return result;
    }

    public override void CreateStatusBar()
    {
        for (var y = 0; y < ViewportHeight; y++)
        {
            DrawStatusLine(y);
        }
    }

    public override void CreateStatusText()
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
            SelectParameter(false, 0x42, 0x15, "Game speed:;FS", State.GameSpeed, null);
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
            DrawChar(0x3E, 0x07, new AnsiChar(elementList.Player().Character, 0x1F));
            DrawChar(0x3E, 0x08, new AnsiChar(elementList.Ammo().Character, 0x1B));
            DrawChar(0x3E, 0x09, new AnsiChar(elementList.Torch().Character, 0x16));
            DrawChar(0x3E, 0x0A, new AnsiChar(elementList.Gem().Character, 0x1B));
            DrawChar(0x3E, 0x0C, new AnsiChar(elementList.Key().Character, 0x1F));
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

    public override void CreateStatusWorld()
    {
        DrawStatusLine(0x08);
        DrawString(0x45, 0x08,
            world.Name.Length <= 0 ? facts.UntitledWorldName : world.Name, 0x1F);
    }

    public override void DrawChar(int x, int y, AnsiChar ac) => 
        terminal.Plot(x, y, ac);

    public override void DrawMessage(IMessage message, int color)
    {
        var text = message.Text.FirstOrDefault();
        if (string.IsNullOrEmpty(text))
            return;

        var x = (60 - text.Length) / 2;
        DrawString(x, 24, " ", text, " ", color);
    }

    public override void DrawPausing() => 
        DrawString(0x40, 0x05, "Pausing...", 0x1F);

    public override void DrawStatusLine(int y)
    {
        var blankChar = new AnsiChar(0x20, 0x11);
        for (var x = 60; x < 80; x++)
        {
            terminal.Plot(x, y, blankChar);
        }
    }

    public void DrawString(int x, int y, ReadOnlySpan<char> text, int color) => 
        terminal.Write(x, y, text, color);

    private void DrawString(int x, int y, ReadOnlySpan<char> text0, ReadOnlySpan<char> text1, int color) => 
        terminal.Write(x, y, text0, text1, color);

    private void DrawString(int x, int y, ReadOnlySpan<char> text0, ReadOnlySpan<char> text1, ReadOnlySpan<char> text2, int color) => 
        terminal.Write(x, y, text0, text1, text2, color);

    private void DrawTileAt(Location location) => 
        DrawTileCommon(location.X, location.Y, boardUpdater.Draw(location + 1));

    private void DrawTileCommon(int x, int y, AnsiChar ac) => 
        playField.DrawTile(x, y, ac);

    public override void DrawTitleStatus() => 
        DrawString(0x3E, 0x05, "Pick a command:", 0x1B);

    public override void FadeBoard(AnsiChar ac) => fadeMatrix.FadeOut(ac);

    private void RandomizeFadeMatrix() => fadeMatrix.Randomize();

    public override void Initialize()
    {
        RandomizeFadeMatrix();
        terminal.SetSize(State.EditorMode ? 60 : 80, 25, false);
    }

    public override void RedrawBoard() => fadeMatrix.FadeIn();

    public override void UpdateBorder()
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

    public override void UpdateStatus()
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
                    ? new AnsiChar(elementList.Key().Character, 0x18 + i)
                    : new AnsiChar(0x20, 0x1F));
        }

        DrawString(0x41, 0x0F, State.GameQuiet ? " Be noisy" : " Be quiet", 0x1F);

        if (world.Flags.Contains("DEBUG"))
            DrawString(0x3E, 0x04, "Used: ", Engine.MemoryUsage.ToCharSpan(buffer), 0x1E);
    }

    public override string EnterCheat()
    {
        DrawStatusLine(4);
        DrawStatusLine(5);
        var cheat = textEntryHud.Show(0x3F, 0x04, 11, 0x0F, 0x1F, ReadOnlySpan<char>.Empty);
        DrawStatusLine(4);
        DrawStatusLine(5);
        return cheat;
    }

    public override int SelectParameter(bool performSelection, int x, int y, string message, int currentValue,
        string? barText) =>
        choiceHud.Show(performSelection, x, y, message, currentValue, barText);

    public override string? EnterHighScore(IHighScoreList highScoreList, int score)
    {
        var index = -1;
            
        var nameList = new List<string>
        {
            "Score  Name",
            "-----  ----------------------------------"
        };

        var nameIndex = 2;
            
        foreach (var hs in highScoreList)
        {
            if (score > 0 && index < 0 && hs.Score <= score)
            {
                index = nameIndex;
                nameList.Add($"{score,5}  -- You! --");
            }

            if (string.IsNullOrEmpty(hs.Name))
                continue;

            nameList.Add($"{hs.Score,5}  {hs.Name}");
            nameIndex++;
        }

        if (index >= 0)
        {
            string? name = null;
            Scroll.Show($"New high score for {world.Name}",
                nameList,
                false,
                2,
                _ => name = longTextEntryHud.Show("Congratulations!  Enter your name:", 3, 18, 34, 0x4E, 0x4F));
            return name;
        }

        Scroll.Show($"High scores for {world.Name}", nameList, false, 0);
        return null;
    }

    public override void ShowHighScores(IHighScoreList highScoreList)
    {
        var nameList = new List<string>
        {
            "Score  Name",
            "-----  ----------------------------------"
        };
            
        nameList.AddRange(
            highScoreList
                .Where(hs => !string.IsNullOrEmpty(hs.Name))
                .Select(hs => $"{hs.Score,5}  {hs.Name}"));

        Scroll.Show($"High scores for {world.Name}", nameList, false, 0);
    }
    
    public override string SaveGame()
    {
        DrawString(65, 3, "Save game:", 0x1F);
        DrawString(71, 5, ".SAV", 0x0F);
        var result = textEntryHud.Show(63, 4, 8, 0x0F, 0x1F, State.DefaultSaveName);
        DrawStatusLine(3);
        DrawStatusLine(5);
        return result;
    }

    public override void FailToLoadWorld()
    {
        DrawString(62, 4, "You need a newer", 0x1E);
        DrawString(62, 5, " version of ZZT!", 0x1E);
        soundUnit.PlayErrorSound();
        Engine.Delay(2000);
    }
}