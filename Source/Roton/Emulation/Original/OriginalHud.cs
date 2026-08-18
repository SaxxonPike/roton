using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalHud : Hud
{
    public OriginalHud(
        IEngineAccessor engine,
        ITerminal terminal,
        IScroll scroll,
        ITextEntryHud textEntryHud,
        IChoiceHud choiceHud,
        ILongTextEntryHud longTextEntryHud,
        IFadeMatrix fadeMatrix)
        : base(engine, scroll)
    {
        Terminal = terminal;
        TextEntryHud = textEntryHud;
        ChoiceHud = choiceHud;
        LongTextEntryHud = longTextEntryHud;
        FadeMatrix = fadeMatrix;
        fadeMatrix.Initialize();
    }

    private IFadeMatrix FadeMatrix { [DebuggerStepThrough] get; } 
    
    private ITerminal Terminal { [DebuggerStepThrough] get; }

    private ITextEntryHud TextEntryHud { [DebuggerStepThrough] get; }

    private IChoiceHud ChoiceHud { [DebuggerStepThrough] get; }

    private ILongTextEntryHud LongTextEntryHud { [DebuggerStepThrough] get; }

    private const int ViewportHeight = 25;

    private const int ViewportWidth = 60;

    public override void ClearPausing()
    {
        DrawStatusLine(5);
    }

    public override void ClearTitleStatus()
    {
        DrawStatusLine(6);
    }

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
        DrawString(0x3D, 0, @"    - - - - -      ", 0x1F);
        DrawString(0x3E, 1, @"     Roton     ", 0x70);
        DrawString(0x3D, 2, @"    - - - - -      ", 0x1F);
        if (Engine.TitleScreen)
        {
            SelectParameter(false, 0x42, 0x15, @"Game speed:;FS", Engine.State.GameSpeed, null);
            DrawString(0x3E, 0x15, @" S ", 0x70);
            DrawString(0x3E, 0x07, @" W ", 0x30);
            DrawString(0x41, 0x07, @" World:", 0x1E);
            CreateStatusWorld();
            DrawString(0x3E, 0x0B, @" P ", 0x70);
            DrawString(0x41, 0x0B, @" Play", 0x1F);
            DrawString(0x3E, 0x0C, @" R ", 0x30);
            DrawString(0x41, 0x0C, @" Restore game", 0x1E);
            DrawString(0x3E, 0x0D, @" Q ", 0x70);
            DrawString(0x41, 0x0D, @" Quit", 0x1E);
            DrawString(0x3E, 0x10, @" A ", 0x30);
            DrawString(0x41, 0x10, @" About Roton!", 0x1F);
            DrawString(0x3E, 0x11, @" H ", 0x70);
            DrawString(0x41, 0x11, @" High Scores", 0x1E);
            DrawString(0x3E, 0x12, @" E ", 0x30);
            DrawString(0x41, 0x12, @" Board Editor", 0x1E);
        }
        else
        {
            DrawString(0x40, 0x07, @" Health:", 0x1E);
            DrawString(0x40, 0x08, @"   Ammo:", 0x1E);
            DrawString(0x40, 0x09, @"Torches:", 0x1E);
            DrawString(0x40, 0x0A, @"   Gems:", 0x1E);
            DrawString(0x40, 0x0B, @"  Score:", 0x1E);
            DrawString(0x40, 0x0C, @"   Keys:", 0x1E);
            DrawChar(0x3E, 0x07, new AnsiChar(Engine.ElementList.Player().Character, 0x1F));
            DrawChar(0x3E, 0x08, new AnsiChar(Engine.ElementList.Ammo().Character, 0x1B));
            DrawChar(0x3E, 0x09, new AnsiChar(Engine.ElementList.Torch().Character, 0x16));
            DrawChar(0x3E, 0x0A, new AnsiChar(Engine.ElementList.Gem().Character, 0x1B));
            DrawChar(0x3E, 0x0C, new AnsiChar(Engine.ElementList.Key().Character, 0x1F));
            DrawString(0x3E, 0x0E, @" T ", 0x70);
            DrawString(0x41, 0x0E, @" Torch", 0x1F);
            DrawString(0x3E, 0x0F, @" B ", 0x30);
            DrawString(0x3E, 0x10, @" H ", 0x70);
            DrawString(0x41, 0x10, @" Help", 0x1F);
            DrawChar(0x43, 0x12, new AnsiChar(0x20, 0x30));
            DrawChar(0x44, 0x12, new AnsiChar(0x18, 0x30));
            DrawChar(0x45, 0x12, new AnsiChar(0x19, 0x30));
            DrawChar(0x46, 0x12, new AnsiChar(0x1A, 0x30));
            DrawChar(0x47, 0x12, new AnsiChar(0x1B, 0x30));
            DrawChar(0x48, 0x12, new AnsiChar(0x20, 0x30));
            DrawString(0x48, 0x12, @" Move", 0x1F);
            DrawChar(0x44, 0x13, new AnsiChar(0x18, 0x70));
            DrawChar(0x45, 0x13, new AnsiChar(0x19, 0x70));
            DrawChar(0x46, 0x13, new AnsiChar(0x1A, 0x70));
            DrawChar(0x47, 0x13, new AnsiChar(0x1B, 0x70));
            DrawChar(0x48, 0x13, new AnsiChar(0x20, 0x70));
            DrawString(0x3D, 0x13, @" Shift ", 0x70);
            DrawString(0x48, 0x13, @" Shoot", 0x1F);
            DrawString(0x3E, 0x15, @" S ", 0x70);
            DrawString(0x41, 0x15, @" Save game", 0x1F);
            DrawString(0x3E, 0x16, @" P ", 0x30);
            DrawString(0x41, 0x16, @" Pause", 0x1F);
            DrawString(0x3E, 0x17, @" Q ", 0x70);
            DrawString(0x41, 0x17, @" Quit", 0x1F);
        }
    }

    public override void CreateStatusWorld()
    {
        DrawStatusLine(0x08);
        DrawString(0x45, 0x08,
            Engine.World.Name.Length <= 0 ? Engine.Facts.UntitledWorldName : Engine.World.Name, 0x1F);
    }

    public override void DrawChar(int x, int y, AnsiChar ac)
    {
        Terminal.Plot(x, y, ac);
    }

    public override void DrawMessage(IMessage message, int color)
    {
        var text = message.Text.FirstOrDefault();
        if (string.IsNullOrEmpty(text))
            return;

        var x = (60 - text.Length) / 2;
        DrawString(x, 24, $" {text} ", color);
    }

    public override void DrawPausing()
    {
        DrawString(0x40, 0x05, @"Pausing...", 0x1F);
    }

    public override void DrawStatusLine(int y)
    {
        var blankChar = new AnsiChar(0x20, 0x11);
        for (var x = 60; x < 80; x++)
        {
            Terminal.Plot(x, y, blankChar);
        }
    }

    public override void DrawString(int x, int y, ReadOnlySpan<char> text, int color)
    {
        Terminal.Write(x, y, text, color);
    }

    public override void DrawTile(int x, int y, AnsiChar ac)
    {
        DrawTileCommon(x, y, ac);
    }

    private void DrawTileAt(Location location)
    {
        DrawTileCommon(location.X, location.Y, Engine.Draw(location + 1));
    }

    private void DrawTileCommon(int x, int y, AnsiChar ac)
    {
        Terminal.Plot(x, y, ac);
    }

    public override void DrawTitleStatus()
    {
        DrawString(0x3E, 0x05, @"Pick a command:", 0x1B);
    }

    public override void FadeBoard(AnsiChar ac) => FadeMatrix.FadeOut(ac);

    private void RandomizeFadeMatrix() => FadeMatrix.Randomize();

    public override void Initialize()
    {
        RandomizeFadeMatrix();
        Terminal.SetSize(Engine.State.EditorMode ? 60 : 80, 25, false);
    }

    private static string IntToString(int i)
    {
        return $"{i} ";
    }

    public override void RedrawBoard() => FadeMatrix.FadeIn();

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
        if (Engine.TitleScreen)
            return;

        if (Engine.Board.TimeLimit <= 0)
        {
            DrawStatusLine(6);
        }
        else
        {
            DrawString(0x40, 0x06, @"   Time:", 0x1E);
            DrawString(0x48, 0x06, IntToString(Engine.Board.TimeLimit - Engine.World.TimePassed), 0x1E);
        }

        if (Engine.World.Health < 0)
        {
            Engine.World.Health = 0;
        }

        DrawString(0x48, 0x07, IntToString(Engine.World.Health), 0x1E);
        DrawString(0x48, 0x08, IntToString(Engine.World.Ammo), 0x1E);
        DrawString(0x48, 0x09, IntToString(Engine.World.Torches), 0x1E);
        DrawString(0x48, 0x0A, IntToString(Engine.World.Gems), 0x1E);
        DrawString(0x48, 0x0B, IntToString(Engine.World.Score), 0x1E);
        if (Engine.World.TorchCycles > 0)
        {
            for (var i = 2; i <= 5; i++)
            {
                DrawChar(0x49 + i, 0x09,
                    Engine.World.TorchCycles / 40 < i ? new AnsiChar(0xB0, 0x16) : new AnsiChar(0xB1, 0x16));
            }
        }
        else
        {
            DrawString(0x4B, 0x09, @"    ", 0x16);
        }

        for (var i = 1; i <= 7; i++)
        {
            DrawChar(0x47 + i, 0x0C,
                Engine.World.Keys[i - 1]
                    ? new AnsiChar(Engine.ElementList.Key().Character, 0x18 + i)
                    : new AnsiChar(0x20, 0x1F));
        }

        DrawString(0x41, 0x0F, Engine.State.GameQuiet ? @" Be noisy" : @" Be quiet", 0x1F);

        if (Engine.World.Flags.Contains("DEBUG"))
            DrawString(0x3E, 0x04, $"Used: {Engine.MemoryUsage}", 0x1E);
    }

    public override string EnterCheat()
    {
        DrawStatusLine(4);
        DrawStatusLine(5);
        var cheat = TextEntryHud.Show(0x3F, 0x04, 11, 0x0F, 0x1F);
        DrawStatusLine(4);
        DrawStatusLine(5);
        return cheat;
    }

    public override int SelectParameter(bool performSelection, int x, int y, string message, int currentValue,
        string barText)
    {
        return ChoiceHud.Show(performSelection, x, y, message, currentValue, barText);
    }

    public override string EnterHighScore(IHighScoreList highScoreList, int score)
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

            if (!string.IsNullOrEmpty(hs.Name))
            {
                nameList.Add($"{hs.Score,5}  {hs.Name}");
                nameIndex++;                    
            }
        }

        if (index >= 0)
        {
            string name = null;
            Scroll.Show($"New high score for {Engine.World.Name}",
                nameList,
                false,
                2,
                _ => name = LongTextEntryHud.Show("Congratulations!  Enter your name:", 3, 18, 34, 0x4E, 0x4F));
            return name;
        }

        Scroll.Show($"High scores for {Engine.World.Name}", nameList, false, 0);
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

        Scroll.Show($"High scores for {Engine.World.Name}", nameList, false, 0);
    }
    
    public override string SaveGame()
    {
        DrawString(65, 3, "Save game:", 0x1F);
        DrawString(71, 5, ".SAV", 0x0F);
        var result = TextEntryHud.Show(63, 4, 8, 0x0F, 0x1F);
        DrawStatusLine(3);
        DrawStatusLine(5);
        return result;
    }

    public override void FailToLoadWorld()
    {
        DrawString(62, 4, "You need a newer", 0x1E);
        DrawString(62, 5, " version of ZZT!", 0x1E);
        Engine.PlayErrorSound();
        Engine.Delay(2000);
    }
}