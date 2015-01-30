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
            }
            else
            {
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
