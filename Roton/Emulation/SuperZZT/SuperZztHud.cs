using System;
using System.Linq;
using Roton.Core;
using Roton.Emulation.Execution;
using Roton.Extensions;

namespace Roton.Emulation.SuperZZT
{
    public sealed class SuperZztHud : Hud
    {
        private readonly IState _state;
        private readonly IBoard _board;
        private readonly ITiles _tiles;
        private readonly IActors _actors;
        private readonly IWorld _world;
        private readonly IElements _elements;
        private readonly IFlags _flags;
        private readonly ITerminal _terminal;
        private readonly IDrawer _drawer;

        public SuperZztHud(Lazy<IEngine> engine, IState state, IBoard board, ITiles tiles, IActors actors, IWorld world,
            IElements elements, IFlags flags, ITerminal terminal, IDrawer drawer)
            : base(engine)
        {
            _state = state;
            _board = board;
            _tiles = tiles;
            _actors = actors;
            _world = world;
            _elements = elements;
            _flags = flags;
            _terminal = terminal;
            _drawer = drawer;
            OldCamera = new Location16(short.MinValue, short.MinValue);
        }

        private Location16 OldCamera { get; }

        private int ViewportHeight => 25;

        private int ViewportWidth => 40;

        protected override bool Confirm(string message)
        {
            DrawString(0x10, 0x18, message, 0x1F);
            DrawChar(0x10 + message.Length, 0x18, new AnsiChar(0x5F, 0x9E));
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
            _terminal.Plot(x, y, ac);
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
            _terminal.Write(x, y, text, color);
        }

        public override void DrawTile(int x, int y, AnsiChar ac)
        {
            DrawTileCommon(x, y, ac);
        }

        private void DrawTileCommon(int x, int y, AnsiChar ac)
        {
            if (_state.EditorMode)
            {
                if (x >= 0 && x < 96 && y >= 0 && y < 80)
                {
                    _terminal.Plot(x, y, ac);
                }
            }
            else
            {
                x += 0x0E + 1;
                y += 0x02 + 1;
                x -= _board.Camera.X;
                y -= _board.Camera.Y;
                if (x >= 0x0E && x <= 0x25 && y >= 0x02 && y <= 0x15)
                {
                    _terminal.Plot(x, y, ac);
                }
            }
        }

        private Vector GetTranslation()
        {
            return new Vector(0x0F + -_board.Camera.X, 0x03 + -_board.Camera.Y);
        }

        public override void Initialize()
        {
            if (_state.EditorMode)
            {
                _terminal.SetSize(96, 80, true);
            }
            else
            {
                _terminal.SetSize(40, 25, true);
            }
        }

        public override void RedrawBoard()
        {
            for (var x = 0; x < _tiles.Width; x++)
            {
                for (var y = 0; y < _tiles.Height; y++)
                {
                    //DrawTile(x, y, _drawer.Draw(x + 1, y + 1));
                    _drawer.UpdateBoard(new Location(x, y));
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
            camera.CopyFrom(_actors.Player.Location);
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

            if (!OldCamera.Matches(camera))
            {
                // Super ZZT does a smart redraw
                // todo: implement that instead of redrawing everything
                OldCamera.CopyFrom(camera);
                _board.Camera.CopyFrom(camera);
                RedrawBoard();
            }
        }

        public override void UpdateStatus()
        {
            if (!Engine.TitleScreen)
            {
                if (_world.Health < 0)
                {
                    _world.Health = 0;
                }

                var healthRemaining = _world.Health;
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

                DrawNumber(0x11, _world.Gems);
                DrawNumber(0x12, _world.Ammo);
                DrawNumber(0x15, _world.Score);
                DrawString(0x00, 0x16, @"            ", 0x6F);

                if (!string.IsNullOrWhiteSpace(_flags.StoneText))
                {
                    DrawString(0x01, 0x16, _flags.StoneText, 0x6F);
                }

                if (_world.Stones >= 0)
                {
                    DrawNumber(0x16, _world.Stones);
                }

                for (var i = 0; i < 7; i++)
                {
                    var keyChar = _world.Keys[i] ? _elements[0x08].Character : 0x20;
                    var x = i & 0x3;
                    var y = x >> 2;
                    DrawChar(0x07 + x, 0x13 + y, new AnsiChar(keyChar, 0x69 + i));
                }

                DrawString(0x03, 0x0A, _state.GameQuiet ? @"Be Noisy " : @"Be Quiet ", 0x6E);
            }
        }
    }
}