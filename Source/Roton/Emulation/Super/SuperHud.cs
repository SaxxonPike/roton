﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Roton.Emulation.Core;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Super
{
    [Context(Context.Super)]
    public sealed class SuperHud : Hud
    {
        private readonly Lazy<ITerminal> _terminal;
        private readonly Lazy<ITextEntryHud> _textEntryHud;

        public SuperHud(Lazy<IEngine> engine, Lazy<ITerminal> terminal, Lazy<IScroll> scroll, Lazy<ITextEntryHud> textEntryHud)
            : base(engine, scroll)
        {
            _terminal = terminal;
            _textEntryHud = textEntryHud;
            OldCamera = new Location16(short.MinValue, short.MinValue);
        }

        private ITerminal Terminal
        {
            [DebuggerStepThrough] get => _terminal.Value;
        }

        private ITextEntryHud TextEntryHud
        {
            [DebuggerStepThrough] get => _textEntryHud.Value;
        }

        private Location16 OldCamera { get; }

        private const int ViewportHeight = 25;

        private const int ViewportWidth = 40;

        protected override bool Confirm(string message)
        {
            UpdateBorder();
            DrawString(0x0F, 0x18, message, 0x1F);
            DrawChar(0x0F + message.Length, 0x18, new AnsiChar(0x5F, 0x9E));
            var result = base.Confirm(message);
            UpdateBorder();
            return result;
        }

        public override void CreateStatusBar()
        {
            for (var y = 0; y < ViewportHeight; y++)
            {
                DrawString(0, y, new string(' ', ViewportWidth), 0x1F);
            }
        }

        public override void CreateStatusText()
        {
            CreateStatusBar();
            if (Engine.TitleScreen)
            {
                DrawString(0x04, 0x0A, @"Press", 0x1E);
                DrawString(0x04, 0x0C, @"ENTER", 0x1F);
                DrawString(0x01, 0x0E, @"to continue", 0x1E);
            }
            else
            {
                var arrows = new string(new[]
                {
                    0x18.ToChar(),
                    0x19.ToChar(),
                    0x1A.ToChar(),
                    0x1B.ToChar()
                });
                DrawString(0x00, 0x00, new string(0xDC.ToChar(), 12), 0x1D);
                DrawString(0x00, 0x01, @"  Commands  ", 0x6F);
                DrawString(0x00, 0x02, new string(0xDF.ToChar(), 12), 0x6D);
                DrawString(0x00, 0x03, $@" {arrows}       ", 0x6F);
                DrawString(0x00, 0x04, @"   Move     ", 0x6E);
                DrawString(0x00, 0x05, $@" Shift+{arrows} ", 0x6F);
                DrawString(0x00, 0x06, @"   Shoot    ", 0x6B);
                DrawString(0x00, 0x07, @"   Hint     ", 0x6E);
                DrawString(0x01, 0x07, @"H", 0x6F);
                DrawString(0x00, 0x08, @"   Save Game", 0x6B);
                DrawString(0x01, 0x08, @"S", 0x6F);
                DrawString(0x00, 0x09, @"   Restore  ", 0x6E);
                DrawString(0x01, 0x09, @"R", 0x6F);
                DrawString(0x00, 0x0A, @"   Be Quiet ", 0x6E);
                DrawString(0x01, 0x0A, @"B", 0x6F);
                DrawString(0x00, 0x0B, @"   Quit     ", 0x6E);
                DrawString(0x01, 0x0B, @"B", 0x6F);
                DrawString(0x00, 0x0C, new string(0xDC.ToChar(), 12), 0x1D);
                DrawString(0x00, 0x0D, @"   Status   ", 0x6F);
                DrawString(0x00, 0x0E, new string(0xDF.ToChar(), 12), 0x6D);
                DrawString(0x00, 0x0F, @"Health      ", 0x6F);
                DrawString(0x00, 0x10, @"            ", 0x6F);
                DrawString(0x00, 0x11, @" Gems       ", 0x6F);
                DrawChar(0x06, 0x11, new AnsiChar(0x04, 0x62));
                DrawString(0x00, 0x12, @" Ammo       ", 0x6F);
                DrawChar(0x06, 0x12, new AnsiChar(0x84, 0x6B));
                DrawString(0x00, 0x13, @" Keys       ", 0x6F);
                DrawString(0x00, 0x14, @"            ", 0x6F);
                DrawString(0x00, 0x15, @" Score      ", 0x6F);
                DrawString(0x00, 0x16, @"            ", 0x6F);
                DrawString(0x00, 0x17, @"            ", 0x6F);
            }

            CreateStatusWindow();
        }

        private void CreateStatusWindow()
        {
            DrawString(0x0D, 0x01, new string(0xDC.ToChar(), 26), 0x1F);
            DrawString(0x0D, 0x16, new string(0xDF.ToChar(), 1), 0x1F);
            DrawString(0x0E, 0x16, new string(0xDF.ToChar(), 25), 0x7F);

            var column = 0xDB.ToChar() + new string(' ', 24) + 0xDB.ToChar();
            for (var y = 0x02; y <= 0x15; y++)
            {
                DrawString(0x0D, y, column, 0x0F);
                DrawChar(0x27, y + 1, new AnsiChar(0xDE, 0x71));
            }
        }

        public override void DrawChar(int x, int y, AnsiChar ac)
        {
            Terminal.Plot(x, y, ac);
        }

        public override void DrawMessage(IMessage message, int color)
        {
            var topText = message.Text.FirstOrDefault() ?? string.Empty;
            var bottomText = message.Text.Skip(1).FirstOrDefault() ?? string.Empty;
            var topX = 26 - (topText.Length >> 1);
            var bottomX = 26 - (bottomText.Length >> 1);
            var messageColor = (color & 0x0F) | 0x10;
            DrawString(topX, 23, $" {topText} ", messageColor);
            DrawString(bottomX, 24, $" {bottomText} ", messageColor);
        }

        private void DrawNumber(int y, int value)
        {
            var s = value.ToString();
            var x = 11 - s.Length;
            DrawString(0x07, y, @"   ", 0x6E);
            DrawString(x, y, s, 0x6E);
        }

        public override void DrawString(int x, int y, string text, int color)
        {
            Terminal.Write(x, y, text, color);
        }

        public override void DrawTile(int x, int y, AnsiChar ac)
        {
            DrawTileCommon(x, y, ac);
        }

        private void DrawTileCommon(int x, int y, AnsiChar ac)
        {
            if (Engine.State.EditorMode)
            {
                if (x >= 0 && x < 96 && y >= 0 && y < 80)
                {
                    Terminal.Plot(x, y, ac);
                }
            }
            else
            {
                var loc = new Location(x, y).Sum(GetTranslation());
                if (IsWithinCamera(loc))
                    Terminal.Plot(loc.X, loc.Y, ac);
            }
        }

        private static bool IsWithinCamera(IXyPair loc)
        {
            return loc.X >= 0x0E && loc.X <= 0x25 && loc.Y >= 0x02 && loc.Y <= 0x15;
        }

        private Vector GetTranslation()
        {
            return new Vector(0x0F + -Engine.Board.Camera.X, 0x03 + -Engine.Board.Camera.Y);
        }

        public override void Initialize()
        {
            if (Engine.State.EditorMode)
            {
                Terminal.SetSize(96, 80, true);
            }
            else
            {
                Terminal.SetSize(40, 25, true);
            }
        }

        public override void RedrawBoard()
        {
            for (var x = 0; x < Engine.Tiles.Width; x++)
            {
                for (var y = 0; y < Engine.Tiles.Height; y++)
                {
                    var loc = new Location(x, y);
                    if (IsWithinCamera(loc.Sum(GetTranslation())))
                        Engine.UpdateBoard(loc.Sum(1, 1));
                }
            }
        }

        public override void UpdateBorder()
        {
            var clearChar = new AnsiChar(0x00, 0x10);
            for (var x = 12; x < 40; x++)
            {
                DrawChar(x, 23, clearChar);
                DrawChar(x, 24, clearChar);
            }
        }

        public override void UpdateCamera()
        {
            var camera = new Location16();
            camera.CopyFrom(Engine.Player.Location);
            camera.Subtract(12, 10);

            if (camera.X < 1)
            {
                camera.X = 1;
            }

            if (camera.X > 73)
            {
                camera.X = 73;
            }

            if (camera.Y < 1)
            {
                camera.Y = 1;
            }

            if (camera.Y > 61)
            {
                camera.Y = 61;
            }

            if (OldCamera.Matches(camera))
                return;

            // todo: implement super smart redraw instead of redrawing everything
            OldCamera.CopyFrom(camera);
            Engine.Board.Camera.CopyFrom(camera);
            RedrawBoard();
        }

        public override void UpdateStatus()
        {
            if (Engine.TitleScreen)
                return;

            if (Engine.World.Health < 0)
            {
                Engine.World.Health = 0;
            }

            var healthRemaining = Engine.World.Health;
            for (var x = 7; x < 12; x++)
            {
                if (healthRemaining >= 20)
                {
                    DrawChar(x, 0x0F, new AnsiChar(0xDB, 0x6E));
                }
                else if (healthRemaining >= 10)
                {
                    DrawChar(x, 0x0F, new AnsiChar(0xDD, 0x6E));
                }
                else
                {
                    DrawChar(x, 0x0F, new AnsiChar(0x20, 0x6E));
                }

                healthRemaining -= 20;
            }

            DrawNumber(0x11, Engine.World.Gems);
            DrawNumber(0x12, Engine.World.Ammo);
            DrawNumber(0x15, Engine.World.Score);
            DrawString(0x00, 0x16, @"            ", 0x6F);

            var stoneText = StoneText;

            if (!string.IsNullOrWhiteSpace(stoneText))
            {
                DrawString(0x01, 0x16, stoneText, 0x6F);
            }

            if (Engine.World.Stones >= 0)
            {
                DrawNumber(0x16, Engine.World.Stones);
            }

            for (var i = 0; i < 7; i++)
            {
                var keyChar = Engine.World.Keys[i] ? Engine.ElementList.Key().Character : 0x20;
                var x = i & 0x3;
                var y = i >> 2;
                DrawChar(0x07 + x, 0x13 + y, new AnsiChar(keyChar, 0x69 + i));
            }

            DrawString(0x03, 0x0A, Engine.State.GameQuiet ? @"Be Noisy " : @"Be Quiet ", 0x6E);
            
            if (Engine.World.Flags.Contains("DEBUG"))
                DrawString(0x0E, 0x00, $"Used: {Engine.MemoryUsage}", 0x1E);            
        }

        private string StoneText
        {
            get
            {
                foreach (var flag in Engine.World.Flags.Select(f => f.ToUpperInvariant()))
                {
                    if (flag.Length > 0 && flag.StartsWith("Z"))
                    {
                        return flag.Substring(1);
                    }
                }

                return string.Empty;
            }
        }

        public override string EnterCheat()
        {
            UpdateBorder();
            var cheat = TextEntryHud.Show(0x0F, 0x17, 11, 0x0F, 0x1F);
            UpdateBorder();
            return cheat;
        }

        public override string EnterHighScore(IHighScoreList highScoreList, int score)
        {
            if (!highScoreList.Any(hs => hs.Score <= score))
            {
                return null;                
            }
            
            string name = null;
            Scroll.Show($"New high score for {Engine.World.Name}",
                new[] {string.Empty, " Enter your name:", string.Empty, string.Empty, string.Empty},
                false,
                3,
                s => name = TextEntryHud.Show(12, 14, 15, 0x1E, 0x1F));
            return name;
        }

        public override void ShowHighScores(IHighScoreList highScoreList)
        {
            var nameList = new List<string>
            {
                "Score  Name",
                "-----  --------------------"
            };
            
            nameList.AddRange(
                highScoreList
                    .Where(hs => !string.IsNullOrEmpty(hs.Name))
                    .Select(hs => $"{hs.Score.ToString().PadLeft(5)}  {hs.Name}"));

            Scroll.Show($"High scores for {Engine.World.Name}", nameList, false, 0);
        }
    }
}