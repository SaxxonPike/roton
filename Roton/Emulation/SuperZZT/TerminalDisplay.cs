using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roton.Emulation.SuperZZT
{
    sealed internal class TerminalDisplay : Display
    {
        public TerminalDisplay(IDisplayInfo infoSource)
            : base(infoSource)
        {
        }

        public override void CreateStatusBar()
        {
            for (int y = 0; y < ViewportHeight; y++)
            {
                DrawString(0, y, new String(' ', ViewportWidth), 0x1F);
            }
        }

        public override void CreateStatusText()
        {
            CreateStatusBar();
            if (DisplayInfo.TitleScreen)
            {
                DrawString(0x04, 0x0A, @"Press", 0x1E);
                DrawString(0x04, 0x0C, @"ENTER", 0x1F);
                DrawString(0x01, 0x0E, @"to continue", 0x1E);
            }
            else
            {
                var arrows = new string(new char[] {
                    (0x18).ToChar(),
                    (0x19).ToChar(),
                    (0x1A).ToChar(),
                    (0x1B).ToChar(),
                });
                DrawString(0x00, 0x00, new string((0xDC).ToChar(), 12), 0x1D);
                DrawString(0x00, 0x01, @"  Commands  ", 0x6F);
                DrawString(0x00, 0x02, new string((0xDF).ToChar(), 12), 0x6D);
                DrawString(0x00, 0x03, @" " + arrows + @"       ", 0x6F);
                DrawString(0x00, 0x04, @"   Move     ", 0x6E);
                DrawString(0x00, 0x05, @" Shift+" + arrows + @" ", 0x6F);
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
                DrawString(0x00, 0x0C, new string((0xDC).ToChar(), 12), 0x1D);
                DrawString(0x00, 0x0D, @"   Status   ", 0x6F);
                DrawString(0x00, 0x0E, new string((0xDF).ToChar(), 12), 0x6D);
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

        void CreateStatusWindow()
        {
            DrawString(0x0D, 0x01, new String((0xDC).ToChar(), 26), 0x1F);
            DrawString(0x0D, 0x16, new String((0xDF).ToChar(), 1), 0x1F);
            DrawString(0x0E, 0x16, new String((0xDF).ToChar(), 25), 0x7F);

            string column = (0xDB).ToChar().ToString() + new String(' ', 24) + (0xDB).ToChar().ToString();
            for (int y = 0x02; y <= 0x15; y++)
            {
                DrawString(0x0D, y, column, 0x0F);
                DrawChar(0x27, y + 1, new AnsiChar(0xDE, 0x71));
            }
        }

        public override void DrawChar(int x, int y, AnsiChar ac)
        {
            Terminal.Plot(x, y, ac);
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
            DrawTileCommon(location.X, location.Y, DisplayInfo.Draw(location));
        }

        private void DrawTileCommon(int x, int y, AnsiChar ac)
        {
            if (x >= 0x0E && x <= 0x25 && y >= 0x02 && y <= 0x15)
            {
                Terminal.Plot(x, y, ac);
            }
        }

        Vector GetTranslation()
        {
            return new Vector(0x0F + (-DisplayInfo.Camera.X), 0x03 + (-DisplayInfo.Camera.Y));
        }

        public override void RedrawBoard()
        {
            for (int x = 0; x < DisplayInfo.Width; x++)
            {
                for (int y = 0; y < DisplayInfo.Height; y++)
                {
                    DrawTile(x, y, DisplayInfo.Draw(new Location(x + 1, y + 1)));
                }
            }
        }

        int ViewportHeight
        {
            get { return 25; }
        }

        int ViewportWidth
        {
            get { return 40; }
        }
    }
}
