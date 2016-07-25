using System;
using System.Linq;
using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.ZZT
{
    internal sealed class ZztHud : Hud
    {
        public ZztHud(IEngine engine, ITerminal terminal)
            : base(engine, terminal)
        {
            FadeMatrix = new Location[ViewportTileCount];
            GenerateFadeMatrix();
        }

        private Location[] FadeMatrix { get; }

        private int ViewportHeight => 25;

        private int ViewportTileCount => ViewportWidth*ViewportHeight;

        private int ViewportWidth => 60;

        public override void ClearPausing()
        {
            DrawStatusLine(5);
        }

        public override void ClearTitleStatus()
        {
            DrawStatusLine(6);
        }

        public override bool Confirm(string message)
        {
            DrawStatusLine(3);
            DrawStatusLine(4);
            DrawStatusLine(5);
            DrawString(0x3F, 0x05, message, 0x1F);
            DrawChar(0x3F + message.Length, 0x05, new AnsiChar(0x5F, 0x9E));
            var key = 0;
            while (key == 0)
            {
                Engine.WaitForTick();
                key = Engine.ReadKey().ToUpperCase();
            }
            var result = key.ToAscii() == @"Y";
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
            DrawString(0x3E, 1, @"      ZZT      ", 0x70);
            DrawString(0x3D, 2, @"    - - - - -      ", 0x1F);
            if (Engine.TitleScreen)
            {
                SelectParameter(false, 0x42, 0x15, @"Game speed:;FS", Engine.StateData.GameSpeed);
                DrawString(0x3E, 0x15, @" S ", 0x70);
                DrawString(0x3E, 0x07, @" W ", 0x30);
                DrawString(0x41, 0x07, @" World:", 0x1E);
                DrawString(0x45, 0x08, Engine.WorldData.Name.Length <= 0 ? @"Untitled" : Engine.WorldData.Name, 0x1F);
                DrawString(0x3E, 0x0B, @" P ", 0x70);
                DrawString(0x41, 0x0B, @" Play", 0x1F);
                DrawString(0x3E, 0x0C, @" R ", 0x30);
                DrawString(0x41, 0x0C, @" Restore game", 0x1E);
                DrawString(0x3E, 0x0D, @" Q ", 0x70);
                DrawString(0x41, 0x0D, @" Quit", 0x1E);
                DrawString(0x3E, 0x10, @" A ", 0x30);
                DrawString(0x41, 0x10, @" About ZZT!", 0x1F);
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
                DrawChar(0x3E, 0x07, new AnsiChar(Engine.Elements[0x04].Character, 0x1F));
                DrawChar(0x3E, 0x08, new AnsiChar(Engine.Elements[0x05].Character, 0x1B));
                DrawChar(0x3E, 0x09, new AnsiChar(Engine.Elements[0x06].Character, 0x16));
                DrawChar(0x3E, 0x0A, new AnsiChar(Engine.Elements[0x07].Character, 0x1B));
                DrawChar(0x3E, 0x0C, new AnsiChar(Engine.Elements[0x08].Character, 0x1F));
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

        public override void DrawChar(int x, int y, AnsiChar ac)
        {
            Terminal.Plot(x, y, ac);
        }

        public override void DrawMessage(IMessage message, int color)
        {
            var text = message.Text.FirstOrDefault();
            if (!string.IsNullOrEmpty(text))
            {
                var x = (60 - text.Length) / 2;
                DrawString(x, 24, $" {text} ", color);
            }
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

        public override void DrawString(int x, int y, string text, int color)
        {
            Terminal.Write(x, y, text, color);
        }

        public override void DrawTile(int x, int y, AnsiChar ac)
        {
            DrawTileCommon(x, y, ac);
        }

        private void DrawTileAt(IXyPair location)
        {
            DrawTileCommon(location.X, location.Y, Engine.Draw(location.Sum(1, 1)));
        }

        private void DrawTileCommon(int x, int y, AnsiChar ac)
        {
            Terminal.Plot(x, y, ac);
        }

        public override void DrawTitleStatus()
        {
            DrawString(0x3E, 0x05, @"Pick a command:", 0x1B);
        }

        public override bool EndGameConfirmation()
        {
            var result = Confirm(@"End this game? ");
            if (Engine.StateData.KeyPressed == 0x1B)
            {
                result = false;
            }
            return result;
        }

        public override void FadeBoard(AnsiChar ac)
        {
            for (var i = 0; i < ViewportTileCount; i++)
            {
                var location = FadeMatrix[i];
                DrawTileCommon(location.X, location.Y, ac);
                FadeWait(i);
            }
        }

        private void FadeWait(int i)
        {
            if ((i & 0x3F) == 0)
            {
                Engine.WaitForTick();
            }
        }

        private void GenerateFadeMatrix()
        {
            // use deterministic randomization here - just as a precaution
            var rnd = new Random(0);
            var index = 0;
            for (var x = 0; x < ViewportWidth; x++)
            {
                for (var y = 0; y < ViewportHeight; y++)
                {
                    FadeMatrix[index++] = new Location(x, y);
                }
            }
            for (var i = 0; i < ViewportTileCount; i++)
            {
                var sourceIndex = i;
                var targetIndex = rnd.Next(FadeMatrix.Length);
                var temp = FadeMatrix[sourceIndex].Clone();
                FadeMatrix[sourceIndex].CopyFrom(FadeMatrix[targetIndex]);
                FadeMatrix[targetIndex].CopyFrom(temp);
            }
        }

        public override void Initialize()
        {
            Terminal.SetSize(80, 25, false);
        }

        private string IntToString(int i)
        {
            return i + " ";
        }

        public override void RedrawBoard()
        {
            for (var i = 0; i < ViewportTileCount; i++)
            {
                var location = FadeMatrix[i];
                DrawTileCommon(location.X, location.Y, Engine.Draw(location.Sum(1, 1)));
                FadeWait(i);
            }
        }

        public override int SelectParameter(bool performSelection, int x, int y, string message, int currentValue)
        {
            string minIndicator;
            string maxIndicator;
            if (message.Length >= 3 && message[message.Length - 3] == ';')
            {
                minIndicator = message[message.Length - 1].ToString();
                maxIndicator = message[message.Length - 2].ToString();
                message = message.Substring(0, message.IndexOf(';') - 1);
            }
            else
            {
                minIndicator = @"1";
                maxIndicator = @"9";
            }
            DrawStatusLine(y);
            DrawString(x, y, message, performSelection ? 0x1F : 0x1E);
            DrawStatusLine(y + 1);
            DrawStatusLine(y + 2);
            DrawString(x, y + 2, minIndicator + @"....:...." + maxIndicator, 0x1E);
            while (true)
            {
                if (performSelection)
                {
                    // todo: selection shit
                    // cancel it for now
                    performSelection = false;
                }
                if (!performSelection || Engine.StateData.KeyShift || Engine.StateData.KeyPressed == 0x0D ||
                    Engine.StateData.KeyPressed == 0x1B)
                {
                    break;
                }
            }
            DrawChar(x + currentValue + 1, y + 1, new AnsiChar(0x1F, 0x1F));
            return currentValue;
        }

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
            if (!Engine.TitleScreen)
            {
                if (Engine.Board.TimeLimit <= 0)
                {
                    DrawStatusLine(6);
                }
                else
                {
                    DrawString(0x40, 0x06, @"   Time:", 0x1E);
                    DrawString(0x48, 0x06, IntToString(Engine.Board.TimeLimit - Engine.WorldData.TimePassed), 0x1E);
                }
                if (Engine.WorldData.Health < 0)
                {
                    Engine.WorldData.Health = 0;
                }
                DrawString(0x48, 0x07, IntToString(Engine.WorldData.Health), 0x1E);
                DrawString(0x48, 0x08, IntToString(Engine.WorldData.Ammo), 0x1E);
                DrawString(0x48, 0x09, IntToString(Engine.WorldData.Torches), 0x1E);
                DrawString(0x48, 0x0A, IntToString(Engine.WorldData.Gems), 0x1E);
                DrawString(0x48, 0x0B, IntToString(Engine.WorldData.Score), 0x1E);
                if (Engine.WorldData.TorchCycles > 0)
                {
                    for (var i = 2; i <= 5; i++)
                    {
                        DrawChar(0x49 + i, 0x09,
                            Engine.WorldData.TorchCycles/40 < i ? new AnsiChar(0xB0, 0x16) : new AnsiChar(0xB1, 0x16));
                    }
                }
                else
                {
                    DrawString(0x4B, 0x09, @"    ", 0x16);
                }

                for (var i = 1; i <= 7; i++)
                {
                    DrawChar(0x47 + i, 0x0C,
                        Engine.WorldData.Keys[i - 1]
                            ? new AnsiChar(Engine.Elements[0x08].Character, 0x18 + i)
                            : new AnsiChar(0x20, 0x1F));
                }

                DrawString(0x41, 0x0F, Engine.StateData.GameQuiet ? @" Be noisy" : @" Be quiet", 0x1F);

                // normally the code would check if debug mode is enabled here
                // and put the m000000 number ZZT generates
                // but that's of no use to us
            }
        }
    }
}