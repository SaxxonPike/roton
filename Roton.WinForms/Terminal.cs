using Roton.Common;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Roton.Common.Resources;
using Roton.Core;

namespace Roton.WinForms
{
    public partial class Terminal : UserControl, IEditorTerminal
    {
        private static readonly Encoding CodePage437 = Encoding.GetEncoding(437);

        private bool _cursorEnabled;
        private int _cursorX;
        private int _cursorY;
        private KeysBuffer _keys;
        private bool _shiftHoldX;
        private bool _shiftHoldY;
        private AnsiChar[] _terminalBuffer;
        private RasterFont _terminalFont;
        private int _terminalHeight;
        private Palette _terminalPalette;
        private bool _terminalWide;
        private int _terminalWidth;
        private bool _updated;

        public Terminal()
        {
            // Initialize a default font/palette in case one isn't defined.
            _terminalFont = new RasterFont();
            _terminalPalette = new Palette();
            Initialize();
        }

        private void Initialize()
        {
            _keys = new KeysBuffer();
            _cursorEnabled = false;
            InitializeComponent();
            Load += OnLoad;
            ScaleX = 1;
            ScaleY = 1;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, true);
        }

        public bool Alt
        {
            get { return _keys.Alt; }
            set { _keys.Alt = value; }
        }

        public void AttachKeyHandler(Form form)
        {
            form.KeyDown += (sender, e) => { OnKey(e); };
            form.KeyUp += (sender, e) => { OnKey(e); };
        }

        private void Blink()
        {
            SuspendLayout();
            Blinking = !Blinking;
            if (_terminalWidth > 0 && _terminalHeight > 0 && (Bitmap != null))
            {
                var total = _terminalWidth*_terminalHeight;
                var i = 0;
                for (var y = 0; y < _terminalHeight; y++)
                {
                    for (var x = 0; x < _terminalWidth; x++)
                    {
                        if ((_terminalBuffer[i].Color & 0x80) != 0)
                        {
                            Draw(x, y, _terminalBuffer[i]);
                        }
                        i++;
                    }
                }
            }
            ResumeLayout();
        }

        private FastBitmap Bitmap { get; set; }

        public bool BlinkEnabled { get; set; }

        private bool Blinking { get; set; }

        public void Clear()
        {
            _terminalBuffer = new AnsiChar[_terminalWidth*_terminalHeight];
            if (Bitmap != null)
            {
                Redraw();
            }
        }

        public void ClearKeys()
        {
            _keys.Clear();
        }

        public bool Control
        {
            get { return _keys.Control; }
            set { _keys.Control = value; }
        }

        public bool CursorEnabled
        {
            get { return _cursorEnabled; }
            set { _cursorEnabled = value; }
        }

        public int CursorX
        {
            get { return _cursorX; }
            set
            {
                _cursorX = value;
                UpdateCursor(_cursorX, _cursorY);
            }
        }

        public int CursorY
        {
            get { return _cursorY; }
            set
            {
                _cursorY = value;
                UpdateCursor(_cursorX, _cursorY);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            timerDaemon.Dispose();
            base.Dispose(disposing);
        }

        public void Draw(int x, int y, AnsiChar ac)
        {
            if (x >= 0 && x < _terminalWidth && y >= 0 && y < _terminalHeight)
            {
                var newHeight = _terminalFont.Height;
                var newWidth = _terminalFont.Width;
                var newX = x*newWidth;
                var newY = y*newHeight;
                var backColor = _terminalPalette.Colors[(ac.Color >> 4) & (BlinkEnabled ? 0x7 : 0xF)];
                var foreColor = BlinkEnabled && Blinking && (ac.Color & 0x80) != 0
                    ? backColor
                    : _terminalPalette.Colors[ac.Color & 0xF];
                _terminalFont.Render(Bitmap, newX, newY, ac.Char, foreColor, backColor);
                _updated = true;
            }
        }

        public IKeyboard Keyboard => _keys as IKeyboard;

        private void OnKey(KeyEventArgs e)
        {
            if (!e.Shift)
            {
                Shift = false;
                _shiftHoldX = false;
                _shiftHoldY = false;
            }
            else
            {
                Shift = true;
            }
            Alt = e.Alt;
            Control = e.Control;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            _keys.Press(e.KeyChar);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            _terminalFont = new Common.RasterFont();
            _terminalPalette = new Palette();
            SetSize(80, 25, false);
            BlinkEnabled = true;
            timerDaemon.Start(Blink, 1f/0.2f);
            displayTimer.Enabled = true;
        }

        private void OnMouse(object sender, MouseEventArgs e)
        {
            if (_cursorEnabled)
            {
                var newX = e.X/_terminalFont.Width;
                var newY = e.Y/_terminalFont.Height;
                UpdateCursor(newX, newY);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            OnMouse(this, e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            OnMouse(this, e);
            base.OnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Bitmap != null)
            {
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                e.Graphics.DrawImageUnscaled(Bitmap, 0, 0);
                UpdateCursor();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        public void Plot(int x, int y, AnsiChar ac)
        {
            if (x >= 0 && x < _terminalWidth && y >= 0 && y < _terminalHeight)
            {
                var index = x + y*_terminalWidth;
                _terminalBuffer[index] = ac;
                Draw(x, y, ac);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            var keyAlt = (keyData & Keys.Alt) != 0;
            var keyControl = (keyData & Keys.Control) != 0;
            var keyRaw = keyData & Keys.KeyCode;
            _keys.NumLock = IsKeyLocked(Keys.NumLock);
            _keys.CapsLock = IsKeyLocked(Keys.CapsLock);

            var result = base.ProcessCmdKey(ref msg, keyData);
            if (!keyAlt && !keyControl && keyRaw != Keys.ShiftKey)
            {
                OnKey(new KeyEventArgs(keyData));
                return _keys.Press(keyData);
            }
            return result;
        }

        public void Redraw()
        {
            var index = 0;
            for (var y = 0; y < _terminalHeight; y++)
            {
                for (var x = 0; x < _terminalWidth; x++)
                {
                    Draw(x, y, _terminalBuffer[index++]);
                }
            }
        }

        public Bitmap RenderSingle(int character, int color)
        {
            color = TranslateColorIndex(color);
            var result = _terminalFont.RenderUnscaled(character, _terminalPalette[color & 0xF],
                _terminalPalette[(color >> 4) & 0xF]);
            if (_terminalWide)
            {
                var wideResult = new Bitmap(result.Width*2, result.Height, result.PixelFormat);
                using (var g = Graphics.FromImage(wideResult))
                {
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    g.DrawImage(result, 0, 0, wideResult.Width, wideResult.Height);
                }
                result.Dispose();
                return wideResult;
            }
            return result;
        }

        public int ScaleX { get; private set; }

        public int ScaleY { get; private set; }

        public void SetScale(int xScale, int yScale)
        {
            ScaleX = xScale;
            ScaleY = yScale;
            SetSize(_terminalWidth, _terminalHeight, _terminalWide);
        }

        public void SetSize(int width, int height, bool wide)
        {
            var oldWidth = _terminalWidth;
            var oldHeight = _terminalHeight;
            _terminalWidth = width;
            _terminalHeight = height;

            _terminalFont.SetScale((wide ? 2 : 1)*ScaleX, ScaleY);
            _terminalWide = wide;

            var oldBitmap = Bitmap;
            Bitmap = new FastBitmap(_terminalFont.Width*_terminalWidth*(_terminalWide ? 2 : 1),
                _terminalFont.Height*_terminalHeight);
            oldBitmap?.Dispose();

            if (width != oldWidth || height != oldHeight)
            {
                Clear();
            }

            if (AutoSize)
            {
                Width = _terminalFont.Width*_terminalWidth;
                Height = _terminalFont.Height*_terminalHeight;
            }

            Redraw();
        }

        public bool Shift
        {
            get { return _keys.Shift; }
            set { _keys.Shift = value; }
        }

        public Common.RasterFont TerminalFont
        {
            get { return _terminalFont; }
            set
            {
                _terminalFont = value;
                Redraw();
            }
        }

        public Palette TerminalPalette
        {
            get { return _terminalPalette; }
            set
            {
                _terminalPalette = value;
                Redraw();
            }
        }

        internal int TranslateColorIndex(int color)
        {
            // if blinking is enabled, we only get the first 8 colors for background
            if (BlinkEnabled)
            {
                color &= 0x7F;
            }
            return color;
        }

        private void UpdateCursor()
        {
            if (_cursorEnabled)
            {
                using (var g = CreateGraphics())
                {
                    using (
                        Pen bright = new Pen(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD)),
                            dark = new Pen(Color.FromArgb(0xFF, 0x22, 0x22, 0x22)))
                    {
                        var outerRect = new Rectangle(_cursorX*_terminalFont.Width, _cursorY*_terminalFont.Height,
                            _terminalFont.Width - 1, _terminalFont.Height - 1);
                        g.DrawLines(dark, new[]
                        {
                            new Point(outerRect.Left, outerRect.Bottom),
                            new Point(outerRect.Right, outerRect.Bottom),
                            new Point(outerRect.Right, outerRect.Top)
                        });
                        g.DrawLines(bright, new[]
                        {
                            new Point(outerRect.Left, outerRect.Bottom),
                            new Point(outerRect.Left, outerRect.Top),
                            new Point(outerRect.Right, outerRect.Top)
                        });
                    }
                }
            }
        }

        private void UpdateCursor(int newX, int newY)
        {
            if (Shift && !_shiftHoldX && !_shiftHoldY)
            {
                if (_cursorX == newX && _cursorY != newY)
                    _shiftHoldX = true;
                else if (_cursorX != newX && _cursorY == newY)
                    _shiftHoldY = true;
            }

            if (_shiftHoldX)
            {
                newX = _cursorX;
            }

            if (_shiftHoldY)
            {
                newY = _cursorY;
            }

            if (newX != _cursorX || newY != _cursorY)
            {
                _updated = true;
                _cursorX = newX;
                _cursorY = newY;
                UpdateCursor();
            }
        }

        public void Write(int x, int y, string value, int color)
        {
            var ac = new AnsiChar {Color = color};
            var characters = CodePage437.GetBytes(value);
            var count = characters.Length;

            while (x < 0)
            {
                x += _terminalWidth;
                y--;
            }

            for (var index = 0; index < count; index++)
            {
                ac.Char = characters[index];
                Plot(x, y, ac);
                x++;
                if (x >= _terminalWidth)
                {
                    x -= _terminalWidth;
                    y++;
                }
            }
        }

        private void displayTimer_Tick(object sender, EventArgs e)
        {
            if (_updated)
            {
                _updated = false;
                Invalidate();
            }
        }
    }
}