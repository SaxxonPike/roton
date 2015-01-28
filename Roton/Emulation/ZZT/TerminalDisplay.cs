using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.ZZT
{
    sealed internal class TerminalDisplay : Display
    {
        public TerminalDisplay(IDisplayInfo infoSource)
            : base(infoSource)
        {
            FadeMatrix = new Location[ViewportTileCount];
        }

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
            int key = 0;
            while (key == 0)
            {
                DisplayInfo.WaitForTick();
                key = DisplayInfo.ReadKey().ToUpperCase();
            }
            bool result = (key.ToAscii() == @"Y");
            DrawStatusLine(5);
            return result;
        }

        public override void CreateStatusBar()
        {
            for (int y = 0; y < ViewportHeight; y++)
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
            if (DisplayInfo.TitleScreen)
            {
                SelectParameter(false, 0x42, 0x15, @"Game speed:;FS", DisplayInfo.GameSpeed);
                DrawString(0x3E, 0x15, @" S ", 0x70);
                DrawString(0x3E, 0x07, @" W ", 0x30);
                DrawString(0x41, 0x07, @" World:", 0x1E);
                if (DisplayInfo.WorldName.Length <= 0)
                {
                    DrawString(0x45, 0x08, @"Untitled", 0x1F);
                }
                else
                {
                    DrawString(0x45, 0x08, DisplayInfo.WorldName, 0x1F);
                }
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
                DrawChar(0x3E, 0x07, new AnsiChar(DisplayInfo.Elements[0x04].Character, 0x1F));
                DrawChar(0x3E, 0x08, new AnsiChar(DisplayInfo.Elements[0x05].Character, 0x1B));
                DrawChar(0x3E, 0x09, new AnsiChar(DisplayInfo.Elements[0x06].Character, 0x16));
                DrawChar(0x3E, 0x0A, new AnsiChar(DisplayInfo.Elements[0x07].Character, 0x1B));
                DrawChar(0x3E, 0x0C, new AnsiChar(DisplayInfo.Elements[0x08].Character, 0x1F));
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

        public override void DrawMessage(string message, int color)
        {
            int x = (60 - message.Length) / 2;
            DrawString(x, 24, " " + message + " ", color);
        }

        public override void DrawPausing()
        {
            DrawString(0x40, 0x05, @"Pausing...", 0x1F);
        }

        public override void DrawStatusLine(int y)
        {
            var blankChar = new AnsiChar(0x20, 0x11);
            for (int x = 60; x < 80; x++)
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

        void DrawTileAt(Location location)
        {
            DrawTileCommon(location.X, location.Y, DisplayInfo.Draw(location.Sum(1, 1)));
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
            bool result = Confirm(@"End this game? ");
            if (DisplayInfo.KeyPressed == 0x1B)
            {
                result = false;
            }
            return result;
        }

        public override void FadeBoard(AnsiChar ac)
        {
            for (int i = 0; i < ViewportTileCount; i++)
            {
                var location = FadeMatrix[i];
                DrawTileCommon(location.X, location.Y, ac);
                FadeWait(i);
            }
        }

        private Location[] FadeMatrix
        {
            get;
            set;
        }

        private void FadeWait(int i)
        {
            if ((i & 0x3F) == 0)
            {
                DisplayInfo.WaitForTick();
            }
        }

        public override void GenerateFadeMatrix()
        {
            // use deterministic randomization here - just as a precaution
            Random rnd = new Random(0);
            int index = 0;
            for (int x = 0; x < ViewportWidth; x++)
            {
                for (int y = 0; y < ViewportHeight; y++)
                {
                    FadeMatrix[index++] = new Location(x, y);
                }
            }
            for (int i = 0; i < ViewportTileCount; i++)
            {
                int sourceIndex = i;
                int targetIndex = rnd.Next(FadeMatrix.Length);
                Location temp = FadeMatrix[sourceIndex].Clone();
                FadeMatrix[sourceIndex].CopyFrom(FadeMatrix[targetIndex]);
                FadeMatrix[targetIndex].CopyFrom(temp);
            }
        }

        private string IntToString(int i)
        {
            return i.ToString() + " ";
        }

        public override void RedrawBoard()
        {
            for (int i = 0; i < ViewportTileCount; i++)
            {
                var location = FadeMatrix[i];
                DrawTileCommon(location.X, location.Y, DisplayInfo.Draw(location.Sum(1, 1)));
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
                if (!performSelection || DisplayInfo.KeyShift || DisplayInfo.KeyPressed == 0x0D || DisplayInfo.KeyPressed == 0x1B)
                {
                    break;
                }
            }
            DrawChar(x + currentValue + 1, y + 1, new AnsiChar(0x1F, 0x1F));
            return currentValue;
        }

        public override void UpdateBorder()
        {
            for (int x = 0; x < ViewportWidth; x++)
            {
                DrawTileAt(new Location(x, 0));
                DrawTileAt(new Location(x, ViewportHeight - 1));
            }
            for (int y = 0; y < ViewportHeight; y++)
            {
                DrawTileAt(new Location(0, y));
                DrawTileAt(new Location(ViewportWidth - 1, y));
            }
        }

        public override void UpdateStatus()
        {
            if (!DisplayInfo.TitleScreen)
            {
                if (DisplayInfo.TimeLimit <= 0)
                {
                    DrawStatusLine(6);
                }
                else
                {
                    DrawString(0x40, 0x06, @"   Time:", 0x1E);
                    DrawString(0x48, 0x06, IntToString(DisplayInfo.TimeLimit - DisplayInfo.TimePassed), 0x1E);
                }
                if (DisplayInfo.Health < 0)
                {
                    DisplayInfo.Health = 0;
                }
                DrawString(0x48, 0x07, IntToString(DisplayInfo.Health), 0x1E);
                DrawString(0x48, 0x08, IntToString(DisplayInfo.Ammo), 0x1E);
                DrawString(0x48, 0x09, IntToString(DisplayInfo.Torches), 0x1E);
                DrawString(0x48, 0x0A, IntToString(DisplayInfo.Gems), 0x1E);
                DrawString(0x48, 0x0B, IntToString(DisplayInfo.Score), 0x1E);
                if (DisplayInfo.TorchCycles > 0)
                {
                    for (int i = 2; i <= 5; i++)
                    {
                        if (DisplayInfo.TorchCycles / 40 < i)
                        {
                            DrawChar(0x49 + i, 0x09, new AnsiChar(0xB0, 0x16));
                        }
                        else
                        {
                            DrawChar(0x49 + i, 0x09, new AnsiChar(0xB1, 0x16));
                        }
                    }
                }
                else
                {
                    DrawString(0x4B, 0x09, @"    ", 0x16);
                }

                for (int i = 1; i <= 7; i++)
                {
                    if (DisplayInfo.Keys[i - 1])
                    {
                        DrawChar(0x47 + i, 0x0C, new AnsiChar(DisplayInfo.Elements[0x08].Character, 0x18 + i));
                    }
                    else
                    {
                        DrawChar(0x47 + i, 0x0C, new AnsiChar(0x20, 0x1F));
                    }
                }

                if (DisplayInfo.Quiet)
                {
                    DrawString(0x41, 0x0F, @" Be noisy", 0x1F);
                }
                else
                {
                    DrawString(0x41, 0x0F, @" Be quiet", 0x1F);
                }

                // normally the code would check if debug mode is enabled here
                // and put the m000000 number ZZT generates
                // but that's of no use to us
            }
        }

        private int ViewportHeight
        {
            get { return 25; }
        }

        private int ViewportTileCount
        {
            get { return ViewportWidth * ViewportHeight; }
        }

        private int ViewportWidth
        {
            get { return 60; }
        }
    }
}
